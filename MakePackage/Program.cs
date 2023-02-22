using MakePackage;
using NuCore.Utilities;

Logger.Init();

bool validArgs = false;

validArgs = CommandLine.Parse(args);

if (validArgs)
{
    // generate a package
    if (!PackageGenerator.StandardRun())
    {
        Logger.Log("A fatal error occurred generating a package.", ConsoleColor.Red, false, false);
    }
    else
    {
        if (CommandLine.InFile == null)
        {
            Logger.Log($"Successfully generated package at {CommandLine.OutFile}!");
        }
        else
        {
            Logger.Log($"Successfully extracted package to {CommandLine.OutFolder}!");
        }
    }

}
else
{
    CommandLine.ShowHelp();
    Environment.Exit(0);
}
