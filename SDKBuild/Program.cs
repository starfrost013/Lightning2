// See https://aka.ms/new-console-template for more information
using NuCore.Utilities;
using System.Diagnostics;
using System.Runtime.InteropServices;

// SDKBuild
// August 11, 2022 (modified October 17, 2022)
//
// Builds the SDK...duh.
// Very quick and dirty

#region Variables
// The release configuration to use.
string config = "Debug";

// Paths for various parts of Lightning.
string buildPath = $@"..\..\..\..\Lightning2\bin\{config}\net6.0\";
string makePackagePath = $@"..\..\..\..\MakePackage\bin\{config}\net6.0\";
string animToolPath = $@"..\..\..\..\AnimTool\bin\{config}\net6.0-windows\";
string docPath = $@"..\..\..\..\Documentation";
string examplePath = $@"..\..\..\..\Examples";
string vsTemplatePath = $@"..\..\..\..\VSTemplate";
string innoInstallDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Inno Setup 6";

#endregion
#region Actual code

NCLogging.Init();

NCLogging.Log("Lightning SDK Builder version 1.6");

NCLogging.Log("Copying build files from LightningGL build directory...");

for (int argId = 0; argId < args.Length; argId++)
{
    string arg = args[argId];
    arg = arg.ToLowerInvariant();

    switch (arg)
    {
        case "-release":
            config = "Release";
            break;
    
    }
}

if (!Directory.Exists(buildPath)) _ = new NCException($"Build directory not found ({buildPath}). Please build Lightning in the specified configuration (provide -release for Release)",
    1402, "buildPath not found in Program::Main");

FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo($@"{buildPath}\LightningBase.dll");
NCLogging.Log($"Building SDK, release {versionInfo.ProductVersion} ({config})");

Directory.CreateDirectory("SDK");

/// top-level statements ???? wtf is it doing here this is the same namespace and class
NCFile.RecursiveCopy(buildPath, "SDK");

// COPY MakePackage
NCLogging.Log("Copying MakePackage build files...");

NCFile.RecursiveCopy(makePackagePath, "SDK");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    NCLogging.Log("Copying AnimTool build files...");

    NCFile.RecursiveCopy(animToolPath, "SDK");
}
else
{
    NCLogging.Log("Not running on windows, skipping AnimTool (remove when MAUI AnimTool is a thing)");
}

// TODO: VSIX Installation
NCLogging.Log("Building documentation...");

if (!Directory.Exists(docPath)) _ = new NCException($"Documentation directory not found ({docPath}", 1401, "docPath not found in Program::Main");

Directory.CreateDirectory(@"SDK\Documentation");

NCFile.RecursiveCopy(docPath, @"SDK\Documentation");

NCLogging.Log("Building examples...");

Directory.CreateDirectory(@"SDK\Examples");

if (!Directory.Exists(examplePath)) _ = new NCException($"Examples directory not found ({examplePath}", 1400, "examplePath not found in Program::Main");

NCFile.RecursiveCopy(examplePath, @"SDK\Examples");

NCLogging.Log("Building VS templates...");

// copy the zip file
Directory.CreateDirectory(@"SDK\VSTemplate");

File.Copy(@$"{vsTemplatePath}\Lightning Game Project.zip", @"SDK\VSTemplate\Lightning Game Project.zip", true);

if (!Directory.Exists(innoInstallDir))
{
    NCLogging.Log("Inno Setup not installed, skipping installer generation phase...");
}
else
{
    NCLogging.Log("Generating installer...");

    Process innoSetup = Process.Start($@"{innoInstallDir}\ISCC.exe", @"..\..\..\..\Setup\Setup.iss");

    // wait for inno to complete
    while (!innoSetup.HasExited) { };

    if (innoSetup.ExitCode > 0)
    {
        NCLogging.Log("Error: Inno Setup failed to generate the SDK installer", ConsoleColor.Red);
    }
}

NCLogging.Log("Done!");

#endregion