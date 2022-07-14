using MakePackage;
using NuCore.Utilities;

// See https://aka.ms/new-console-template for more information

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
        NCLogging.Log($"Successfully generated package at {CommandLine.OutFile}!");
    }

}
else
{
    CommandLine.ShowHelp();
    Environment.Exit(0);
}
