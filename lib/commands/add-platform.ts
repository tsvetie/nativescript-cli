import * as shelljs from "shelljs";
import * as path from "path";

export class AddPlatformCommand implements ICommand {
	constructor(protected $projectData: IProjectData,
		private $platformService: IPlatformService,
		protected $errors: IErrors,
		private $fs: IFileSystem,
		protected $logger: ILogger) { }

	execute(args: string[]): IFuture<void> {
		return (() => {
			if (args[0].toLowerCase() == "vr") {
				let projectPath = this.$projectData.projectDir;
				let unityPath = path.join(projectPath, "platforms", "unity");

				if (!this.$fs.exists(unityPath).wait()) {
					shelljs.mkdir("-p", unityPath);

					shelljs.cp("-R", path.join(__dirname, "..", "..", "resources", "vr", "*"), unityPath);

					this.$logger.printMarkdown("Successfully initialized project for `Virtual Reality` development.");
				}
			} else {
				this.$platformService.addPlatforms(args).wait();
			}
		}).future<void>()();
	}

	allowedParameters: ICommandParameter[] = [];

	canExecute(args: string[]): IFuture<boolean> {
		return (() => {
			if(!args || args.length === 0) {
				this.$errors.fail("No platform specified. Please specify a platform to add");
			}

			_.each(args, arg => arg.toLowerCase() === "vr" || this.$platformService.validatePlatform(arg));

			return true;
		}).future<boolean>()();
	}
}
$injector.registerCommand("platform|add", AddPlatformCommand);
