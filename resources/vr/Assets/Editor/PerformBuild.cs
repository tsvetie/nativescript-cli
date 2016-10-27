using System;
using UnityEditor;
using UnityEngine;
public class BuildService {	
	[MenuItem("Build/Build Android")]
	static void AndroidBuild ()
	{		
		string[] args = System.Environment.GetCommandLineArgs();
		string outputPath = "";
		string appId = "";
		string appName = "";
		 
		for (int i = 0; i < args.Length; i++) {
			if (args [i] == "-outputPath") {
				outputPath = args [i + 1];
			}
			
			if (args [i] == "-appId") {
				appId = args [i + 1];
			}
			
			if (args [i] == "-appName") {
				appName = args [i + 1];
			}
		}
		 
		Debug.Log("Output path: " + outputPath);
		Debug.Log("Application identifier: " + appId);
		Debug.Log("Application identifier: " + appName);
		
		PlayerSettings.bundleIdentifier = appId;
		PlayerSettings.productName = appName;
		 
		UnityEditor.BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, BuildTarget.Android, BuildOptions.None);
	}
}