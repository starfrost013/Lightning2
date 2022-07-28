using MakePackage;
using NuCore.Utilities;

NCLogging.Init();

bool validArgs = false;

validArgs = CommandLine.Parse(args);

if (validArgs)
{
    if (!PackageGenerator.StandardRun())
    {
        NCLogging.Log("A fatal error occurred generating a package.", ConsoleColor.Red, false, false);
    }
    else
    {
        if (CommandLine.InFile == null)
        {
            NCLogging.Log($"Successfully generated package at {CommandLine.OutFile}!");
        }
        else
        {
            NCLogging.Log($"Successfully extracted package to {CommandLine.OutFolder}!");
        }

    }

}
else
{
    CommandLine.ShowHelp();
    Environment.Exit(0);
}
