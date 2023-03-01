// See https://aka.ms/new-console-template for more information
using LightningUtil;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

// SDKBuild 2.2 for Lightning 2.0
// Very quick and dirty tool to build a Lightning SDK setup file.
Logger.Init(); // init NCLogging

#region Variables

// The release configuration to use.
string config = "Debug";
string frameworkVersion = $"{Environment.Version.Major}.{Environment.Version.Minor}";

// If we will run setup or not.
bool runSetup = true;

// Run setup loudly.
bool noQuiet = false;

// Don't measure the time SDKBuild takes to run.
bool noTimeBuild = false;

// Paths for various parts of Lightning.
// (make these overridable...eventually)
string buildPath = $@"..\..\..\..\Lightning2\bin\{config}\net{frameworkVersion}\";
string makePackagePath = $@"..\..\..\..\MakePackage\bin\{config}\net{frameworkVersion}\";
string animToolPath = $@"..\..\..\..\AnimTool\bin\{config}\net{frameworkVersion}-windows\";
string docPath = $@"..\..\..\..\Documentation";
string examplePath = $@"..\..\..\..\Examples";
string vsTemplatePath = $@"..\..\..\..\VSTemplate";
string innoInstallDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Inno Setup 6";
string innoIssPath = @"..\..\..\..\Setup\Setup.iss";
string vsProjectTemplatePath = @"SDK\VSTemplate\Lightning Game Project.zip";
string vsSceneTemplatePath = @"SDK\VSTemplate\Lightning Scene Template.zip";
string finalSdkSetupPath = @"SDK\Setup\SDKSetup.exe";
string finalSdkSetupArgsAllUsers = @"/VERYSILENT /FORCECLOSEAPPLICATIONS /RESTARTAPPLICATIONS /SP-";
string finalSdkSetupArgsCurrentUser = @"/VERYSILENT /FORCECLOSEAPPLICATIONS /RESTARTAPPLICATIONS /SP- /CURRENTUSER";
string finalSdkSetupArgs = finalSdkSetupArgsAllUsers;
Stopwatch stopwatch = Stopwatch.StartNew();

#endregion

#region Command line parsing
for (int argId = 0; argId < args.Length; argId++)
{
    string arg = args[argId];
    arg = arg.ToLowerInvariant();
    arg = arg.Replace("/", "-");

    switch (arg)
    {
        case "-release":
            Logger.Log("Specified Release config (instead of debug)", ConsoleColor.Blue);
            config = "Release";
            break;
        case "-norunsetup":
            Logger.Log("Specified that setup will not be run", ConsoleColor.Blue);
            runSetup = false;
            break;
        case "-noquiet":
            Logger.Log("Specified that setup will be run loudly", ConsoleColor.Blue);
            noQuiet = true;
            break;
        case "-notimebuild":
            Logger.Log("Not measuring time to run SDKBuild", ConsoleColor.Blue);
            noTimeBuild = true;
            break;
        case "-currentuser":
            Logger.Log("Specified install as quiet user", ConsoleColor.Blue);
            finalSdkSetupArgs = finalSdkSetupArgsCurrentUser;
            break;
    }
}

#endregion

#region Actual code

if (!noTimeBuild) stopwatch.Start();

#pragma warning disable IL3000 // in this case it's never embedded in a single file app (THIS REASONING WILL TURN OUT TO BE WRONG! YAY!)
FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
#pragma warning restore IL3000

Logger.Log($"Lightning SDK Builder version {fvi.FileMajorPart}.{fvi.FileMinorPart}.{fvi.FileBuildPart}");

// delete sdk dir if it exists
if (Directory.Exists("SDK"))
{
    Logger.Log("Deleting pre-existing SDK...");
    Directory.Delete("SDK", true);
}

Logger.Log("Copying build files from Lightning build directory...");

if (!Directory.Exists(buildPath))
{
    Logger.LogError($"Build directory not found ({buildPath})!. Please build Lightning in the specified configuration (provide -release for Release, otherwise Debug)", 1402,
        LoggerSeverity.FatalError);
    Environment.Exit(1);
}

FileUtils.RecursiveCopy(buildPath, "SDK");

// build makepackage and animtool first because they may be compiled against older versions of lightning
// so we overwrite old versions if they happen to be there
Logger.Log("Copying MakePackage build files...");

FileUtils.RecursiveCopy(makePackagePath, "SDK");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    Logger.Log("Copying AnimTool build files...");

    FileUtils.RecursiveCopy(animToolPath, "SDK");
}
else
{
    Logger.Log("Not running on windows, skipping AnimTool (remove when MAUI AnimTool is a thing)");
}

FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo($@"{buildPath}\LightningBase.dll");
Logger.Log($"Building SDK, release {versionInfo.ProductVersion} ({config})");

Directory.CreateDirectory("SDK");

/// top-level statements ???? wtf is it doing here this is the same namespace and class
FileUtils.RecursiveCopy(buildPath, "SDK");

// TODO: VSIX Installation
Logger.Log("Building documentation...");

if (!Directory.Exists(docPath)) Logger.LogError($"Documentation directory not found ({docPath}!", 1401, LoggerSeverity.Warning);

Directory.CreateDirectory(@"SDK\Documentation");

FileUtils.RecursiveCopy(docPath, @"SDK\Documentation");

Logger.Log("Building examples...");

Directory.CreateDirectory(@"SDK\Examples");

if (!Directory.Exists(examplePath)) Logger.LogError($"Examples directory not found ({examplePath}!", 1403, LoggerSeverity.Warning);

FileUtils.RecursiveCopy(examplePath, @"SDK\Examples");

Logger.Log("Building VS templates...");

// copy the zip file
Directory.CreateDirectory(@"SDK\VSTemplate");

File.Copy(@$"{vsTemplatePath}\Lightning Game Project.zip", vsProjectTemplatePath, true);
File.Copy(@$"{vsTemplatePath}\Lightning Scene Template.zip", vsSceneTemplatePath, true);


if (!Directory.Exists(innoInstallDir))
{
    Logger.Log("Inno Setup not installed, skipping installer generation phase...");
}
else
{
    Logger.Log("SDK built! Generating installer...", ConsoleColor.Green);

    Process innoSetup = Process.Start($@"{innoInstallDir}\ISCC.exe", innoIssPath);

    // wait for inno to complete
    while (!innoSetup.HasExited) { };

    if (innoSetup.ExitCode > 0)
    {
        Logger.Log($"Error: Inno Setup failed to generate the SDK installer (exit code {innoSetup.ExitCode})!", ConsoleColor.Red);
    }
    else
    {
        if (runSetup)
        {
            Logger.Log("Running generated SDKSetup.exe (You will receive an admin prompt and any applications may be force-closed and restarted)...");

            Process sdkSetup;

            // will always change from default here
            if (noQuiet)
            {
                sdkSetup = Process.Start(finalSdkSetupPath);
            }
            else
            {
                sdkSetup = Process.Start(finalSdkSetupPath, finalSdkSetupArgs);
            }

            // wait for SDKSetup to exit.
            while (!sdkSetup.HasExited) { }; 

            if (sdkSetup.ExitCode > 0) Logger.Log($"Error: Failed to install SDK (exit code {sdkSetup.ExitCode})!", ConsoleColor.Red);
        }

    }
}

// measure how long build took
if (!noTimeBuild)
{
    stopwatch.Stop();
    double instSeconds = (double)stopwatch.ElapsedMilliseconds / 1000;
    Logger.Log($"Build and install took {instSeconds:F1} sec (run setup={runSetup}, loud setup={noQuiet})");
}

Logger.Log("Done!", ConsoleColor.Green);

#endregion