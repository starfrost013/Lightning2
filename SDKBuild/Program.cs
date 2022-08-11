// See https://aka.ms/new-console-template for more information
using NuCore.Utilities;
using System.Diagnostics;

// SDKBuild
// August 11, 2022
//
// Builds the SDK...duh.

NCLogging.Init();

NCLogging.Log("Lightning SDK Builder version 1.0");

NCLogging.Log("Copying build files from LightningGL build directory...");

string buildPath = $@"..\..\..\..\Lightning2\bin\Debug\net6.0\";
string config = "Debug";

if (args.Contains("-release"))
{
    buildPath = $@"..\..\..\..\Lightning2\bin\Release\net6.0\";
    config = "Release";
}

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
        && !finalFilePath.Contains(".tmp"))
    {
        File.Copy(fileName, $@"SDK\{finalFilePath}", true);
    }
}

/// top-level statements ???? wtf is it doing here this is the same namespace and class
NCFile.RecursiveCopy(buildPath, "SDK");

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
string examplePath = $@"..\..\..\..\Lightning2\Content\Examples";

if (!Directory.Exists(examplePath)) _ = new NCException($"Examples directory not found ({examplePath}", 1400, "examplePath not found in Program::Main");
foreach (string fileName in Directory.GetFiles(examplePath))
{
    string finalFilePath = fileName.Replace(examplePath, "");

    if (!finalFilePath.Contains("~$") 
        && !finalFilePath.Contains(".tmp"))
    {
        File.Copy(fileName, $@"SDK\Examples\{finalFilePath}", true);
    }
}

NCFile.RecursiveCopy(examplePath, @"SDK\Examples");

NCLogging.Log("Done!");