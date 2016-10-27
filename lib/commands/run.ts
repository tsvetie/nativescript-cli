import { BuildVRCommand } from "./build"

export class RunCommandBase {
	constructor(private $platformService: IPlatformService,
		private $usbLiveSyncService: ILiveSyncService,
		protected $options: IOptions) { }

	public executeCore(args: string[], buildConfig?: IBuildConfig): IFuture<void> {
		if (this.$options.watch) {
			return this.$usbLiveSyncService.liveSync(args[0]);
		} else {
			return this.$platformService.runPlatform(args[0], buildConfig);
		}
	}
}

export class RunIosCommand extends RunCommandBase implements ICommand {
	constructor($platformService: IPlatformService,
		private $platformsData: IPlatformsData,
		$usbLiveSyncService: ILiveSyncService,
		$options: IOptions) {
		super($platformService, $usbLiveSyncService, $options);
	}

	public allowedParameters: ICommandParameter[] = [];

	public execute(args: string[]): IFuture<void> {
		return this.executeCore([this.$platformsData.availablePlatforms.iOS]);
	}
}
$injector.registerCommand("run|ios", RunIosCommand);

export class RunAndroidCommand extends RunCommandBase implements ICommand {
	constructor($platformService: IPlatformService,
		private $platformsData: IPlatformsData,
		$usbLiveSyncService: ILiveSyncService,
		$options: IOptions,
		private $errors: IErrors) {
		super($platformService, $usbLiveSyncService, $options);
	}

	public allowedParameters: ICommandParameter[] = [];

	public execute(args: string[]): IFuture<void> {
		return this.executeCore([this.$platformsData.availablePlatforms.Android]);
	}

	public canExecute(args: string[]): IFuture<boolean> {
		return (() => {
			if (this.$options.release && (!this.$options.keyStorePath || !this.$options.keyStorePassword || !this.$options.keyStoreAlias || !this.$options.keyStoreAliasPassword)) {
				this.$errors.fail("When producing a release build, you need to specify all --key-store-* options.");
			}
			return args.length === 0;
		}).future<boolean>()();
	}
}
$injector.registerCommand("run|android", RunAndroidCommand);

export class RunVrCommand extends BuildVRCommand {
	constructor(private $devicesService: Mobile.IDevicesService,
		private $androidDeviceDiscovery: Mobile.IDeviceDiscovery,
		private $options: IOptions,
		protected $projectData: IProjectData,
		protected $errors: IErrors,
		$platformService: IPlatformService,
		$fs: IFileSystem,
		protected $logger: ILogger) {
			super($projectData, $platformService, $errors, $fs, $logger);
	}

	public allowedParameters: ICommandParameter[] = [];

	public execute(args: string[]): IFuture<void> {
		return (() => {
			super.execute(args).wait();
			this.$devicesService.initialize({ platform: "android" }).wait();

			this.$devicesService.execute((device: Mobile.IDevice) => {
				return (() => {
					this.$logger.printMarkdown("Installing application on device `" + device.deviceInfo.identifier + "`.");
					device.applicationManager.installApplication(this.pathToApk).wait();
					device.applicationManager.startApplication(this.$projectData.projectId).wait();
					this.$logger.printMarkdown("Successfully started application on device `" + device.deviceInfo.identifier + "`.");
				}).future<void>()();
			}).wait();
		}).future<void>()();
	}

	public canExecute(args: string[]): IFuture<boolean> {
		return (() => {
			if (this.$options.release && (!this.$options.keyStorePath || !this.$options.keyStorePassword || !this.$options.keyStoreAlias || !this.$options.keyStoreAliasPassword)) {
				this.$errors.fail("When producing a release build, you need to specify all --key-store-* options.");
			}
			return args.length === 0;
		}).future<boolean>()();
	}
}
$injector.registerCommand("run|vr", RunVrCommand);
