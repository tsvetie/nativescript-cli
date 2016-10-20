import * as constants from "../../constants";
import * as helpers from "../../common/helpers";
import * as path from "path";
import * as semver from "semver";
import * as fiberBootstrap from "../../common/fiber-bootstrap";

let gaze = require("gaze");

class LiveSyncService implements ILiveSyncService {
	public forceExecuteFullSync = false;
	private _isInitialized = false;

	constructor(private $devicePlatformsConstants: Mobile.IDevicePlatformsConstants,
		private $errors: IErrors,
		private $platformsData: IPlatformsData,
		private $platformService: IPlatformService,
		private $projectData: IProjectData,
		private $projectDataService: IProjectDataService,
		private $prompter: IPrompter,
		private $injector: IInjector,
		private $liveSyncProvider: ILiveSyncProvider,
		private $mobileHelper: Mobile.IMobileHelper,
		private $devicesService: Mobile.IDevicesService,
		private $options: IOptions,
		private $logger: ILogger,
		private $dispatcher: IFutureDispatcher,
		private $hooksService: IHooksService,
		private $processService: IProcessService) { }

	private ensureAndroidFrameworkVersion(platformData: IPlatformData): IFuture<void> { // TODO: this can be moved inside command or canExecute function
		return (() => {
			this.$projectDataService.initialize(this.$projectData.projectDir);
			let frameworkVersion = this.$projectDataService.getValue(platformData.frameworkPackageName).wait().version;

			if (platformData.normalizedPlatformName.toLowerCase() === this.$devicePlatformsConstants.Android.toLowerCase()) {
				if (semver.lt(frameworkVersion, "1.2.1")) {
					let shouldUpdate = this.$prompter.confirm("You need Android Runtime 1.2.1 or later for LiveSync to work properly. Do you want to update your runtime now?").wait();
					if (shouldUpdate) {
						this.$platformService.updatePlatforms([this.$devicePlatformsConstants.Android.toLowerCase()]).wait();
					} else {
						return;
					}
				}
			}
		}).future<void>()();
	}

	public get isInitialized(): boolean { // This function is used from https://github.com/NativeScript/nativescript-dev-typescript/blob/master/lib/before-prepare.js#L4
		return this._isInitialized;
	}

	public liveSync(platform: string, applicationReloadAction?: (deviceAppData: Mobile.IDeviceAppData, localToDevicePaths: Mobile.ILocalToDevicePathData[]) => IFuture<void>): IFuture<void> {
		return (() => {
			let liveSyncData: ILiveSyncData[] = [];
			if (platform) {
				this.$devicesService.initialize({ platform: platform, deviceId: this.$options.device  }).wait();
				liveSyncData.push(this.prepareLiveSyncData(platform));
			} else if (this.$options.device) {
				this.$devicesService.initialize({ platform: platform, deviceId: this.$options.device }).wait();
				platform = this.$devicesService.getDeviceByIdentifier(this.$options.device).deviceInfo.platform;
				liveSyncData.push(this.prepareLiveSyncData(platform));
			} else {
				this.$devicesService.initialize({ skipInferPlatform: true }).wait();
				this.$devicesService.stopDeviceDetectionInterval().wait();
				for(let installedPlatform of this.$platformService.getInstalledPlatforms().wait()) {
					if (this.$devicesService.getDevicesForPlatform(installedPlatform).length === 0) {
						this.$devicesService.startEmulator(installedPlatform).wait();
					}
					liveSyncData.push(this.prepareLiveSyncData(installedPlatform));
				}
			}
            if (liveSyncData.length === 0) {
				this.$errors.fail("There are no platforms installed in this project. Please specify platform or install one by using `tns platform add` command!");
            }
			this._isInitialized = true; // If we want before-prepare hooks to work properly, this should be set after preparePlatform function

			this.liveSyncCore(liveSyncData, applicationReloadAction).wait();
		}).future<void>()();
	}

	private prepareLiveSyncData(platform: string): ILiveSyncData {
		platform = platform || this.$devicesService.platform;
		this.$platformService.preparePlatform(platform.toLowerCase()).wait();
		let platformData = this.$platformsData.getPlatformData(platform.toLowerCase());
		if (this.$mobileHelper.isAndroidPlatform(platform)) {
			this.ensureAndroidFrameworkVersion(platformData).wait();
		}
		let liveSyncData: ILiveSyncData = {
			platform: platform,
			appIdentifier: this.$projectData.projectId,
			projectFilesPath: path.join(platformData.appDestinationDirectoryPath, constants.APP_FOLDER_NAME),
			syncWorkingDirectory: path.join(this.$projectData.projectDir, constants.APP_FOLDER_NAME),
			excludedProjectDirsAndFiles: this.$options.release ? constants.LIVESYNC_EXCLUDED_FILE_PATTERNS : [],
			forceExecuteFullSync: this.forceExecuteFullSync
		};

		return liveSyncData;
	}

	private resolvePlatformLiveSyncBaseService(platform: string, liveSyncData: ILiveSyncData): IPlatformLiveSyncService {
		return this.$injector.resolve(this.$liveSyncProvider.platformSpecificLiveSyncServices[platform.toLowerCase()], { _liveSyncData: liveSyncData });
	}

	@helpers.hook('livesync')
	private liveSyncCore(liveSyncData: ILiveSyncData[], applicationReloadAction: (deviceAppData: Mobile.IDeviceAppData, localToDevicePaths: Mobile.ILocalToDevicePathData[]) => IFuture<void>): IFuture<void> {
		return (() => {
			let watchForChangeActions: ((event: string, filePath: string, dispatcher: IFutureDispatcher) => void)[] = [];
			_.each(liveSyncData, (dataItem) => {
				let service = this.resolvePlatformLiveSyncBaseService(dataItem.platform, dataItem);
				watchForChangeActions.push((event: string, filePath: string, dispatcher: IFutureDispatcher) => {
					service.partialSync(event, filePath, dispatcher, applicationReloadAction);
				});
				service.fullSync(applicationReloadAction).wait();
			});

			if(this.$options.watch) {
				this.$hooksService.executeBeforeHooks('watch').wait();
				this.partialSync(liveSyncData[0].syncWorkingDirectory, watchForChangeActions);
			}
		}).future<void>()();
	}

	private partialSync(syncWorkingDirectory: string, onChangedActions: ((event: string, filePath: string, dispatcher: IFutureDispatcher) => void )[]): void {
		let that = this;

		let gazeWatcher = gaze("**/*", { cwd: syncWorkingDirectory }, function (err: any, watcher: any) {
			this.on('all', (event: string, filePath: string) => {
				fiberBootstrap.run(() => {
					that.$dispatcher.dispatch(() => (() => {
						try {
							for (let i = 0; i < onChangedActions.length; i++) {
								onChangedActions[i](event, filePath, that.$dispatcher);
							}
						} catch (err) {
							that.$logger.info(`Unable to sync file ${filePath}. Error is:${err.message}`.red.bold);
							that.$logger.info("Try saving it again or restart the livesync operation.");
						}
					}).future<void>()());
				});
			});
		});

		this.$processService.attachToProcessExitSignals(this, () => gazeWatcher.close());
		this.$dispatcher.run();
	}
}
$injector.register("usbLiveSyncService", LiveSyncService);
