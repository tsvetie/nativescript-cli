NativeScript Command-Line Interface
================

A fork of the NativeScript Command-Line Interface(https://github.com/NativeScript/nativescript-cli) that implements the following commands:

1. "tns platform add vr"
2. "tns build vr"
3. "tns run vr --justlaunch" - performs "tns platform add vr" if necessary and "tns build vr".

Prerequisites:

1. Install unity - https://unity3d.com/
2. Add to PATH environment variable the path to unity.exe.
3. Download and install the nativescript-2.5.0.tgz from the root of the repository (npm install -g) 

Steps to test the added commands:

1. "tns create [AppName] --tsc"
2. "cd [AppName]"
3. "tns run vr --justlaunch"
