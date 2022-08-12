// See https://aka.ms/new-console-template for more information
using NuCore.Utilities;
using System.Diagnostics;

// SDKBuild
// August 11, 2022 (modified August 12, 2022)
//
// Builds the SDK...duh.
// Very quick and dirty

NCLogging.Init();

NCLogging.Log("Lightning SDK Builder version 1.3");

NCLogging.Log("Copying build files from LightningGL build directory...");

string config = "Debug";

if (args.Contains("-release"))
{
    config = "Release";
}

string buildPath = $@"..\..\..\..\Lightning2\bin\{config}\net6.0\";

if (!Directory.Exists(buildPath)) _ = new NCException($"Build directory not found ({buildPath}). Please build Lightning in the specified configuration (provide -release for Release)", 
    1402, "buildPath not found in Program::Main");

FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo($@"{buildPath}\LightningGL.dll");
NCLogging.Log($"Building SDK, release {versionInfo.ProductVersion} ({config})");

Directory.CreateDirectory("SDK");

foreach (string fileName in Directory.GetFiles(buildPath))
{
    // make the path relative
    string finalFilePath = fileName.Replace(buildPath, "");

    if (!finalFilePath.Contains("~$")
        && !finalFilePath.Contains(".tmp")
        && !finalFilePath.Contains(".pdb"))
    {
        File.Copy(fileName, $@"SDK\{finalFilePath}", true);
    }
}

/// top-level statements ???? wtf is it doing here this is the same namespace and class
NCFile.RecursiveCopy(buildPath, "SDK");

// COPY MakePackage
NCLogging.Log("Copying MakePackage build files...");

string makePackagePath = $@"..\..\..\..\MakePackage\bin\{config}\net6.0\";

foreach (string fileName in Directory.GetFiles(makePackagePath))
{
    // make the path relative
    string finalFilePath = fileName.Replace(makePackagePath, "");

    if (!finalFilePath.Contains("~$")
        && !finalFilePath.Contains(".tmp")
        && !finalFilePath.Contains(".pdb"))
    {
        File.Copy(fileName, $@"SDK\{finalFilePath}", true);
    }
}

NCFile.RecursiveCopy(makePackagePath, "SDK");

NCLogging.Log("Building documentation...");

string docPath = $@"..\..\..\..\Lightning2\Content\Documentation";
if (!Directory.Exists(docPath)) _ = new NCException($"Documentation directory not found ({docPath}", 1401, "docPath not found in Program::Main");

Directory.CreateDirectory(@"SDK\Documentation");
foreach (string fileName in Directory.GetFiles(docPath))
{
    // make the path relative
    string finalFilePath = fileName.Replace(docPath, "");

    if (!finalFilePath.Contains("~$")
        && !finalFilePath.Contains(".tmp"))
    {
        File.Copy(fileName, $@"SDK\Documentation\{finalFilePath}", true);
    }
}

NCFile.RecursiveCopy(docPath, @"SDK\Documentation");

NCLogging.Log("Building examples...");

Directory.CreateDirectory(@"SDK\Examples");
string examplePath = $@"..\..\..\..\Examples";

if (!Directory.Exists(examplePath)) _ = new NCException($"Examples directory not found ({examplePath}", 1400, "examplePath not found in Program::Main");
foreach (string fileName in Directory.GetFiles(examplePath))
{
    string finalFilePath = fileName.Replace(examplePath, "");

    if (!finalFilePath.Contains("~$") 
        && !finalFilePath.Contains(".tmp")
        && !finalFilePath.Contains(".pdb"))
    {
        File.Copy(fileName, $@"SDK\Examples\{finalFilePath}", true);
    }
}

NCFile.RecursiveCopy(examplePath, @"SDK\Examples");

string innoInstallDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\Inno Setup 6";

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