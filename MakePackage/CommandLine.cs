using LightningPackager;
using NuCore.Utilities;

namespace MakePackage
{
    /// <summary>
    /// CommandLine
    /// 
    /// July 11, 2022 (modified July 14, 2022)
    /// 
    /// CommandLine arguments for MakePackage.
    /// </summary>
    public static class CommandLine
    {
        /// <summary>
        /// The input folder to acquire files from.
        /// </summary>
        public static string InFolder { get; set; }

        /// <summary>
        /// The output package file.
        /// </summary>
        public static string OutFile { get; set; }

        /// <summary>
        /// Optional game name.
        /// </summary>
        public static string Name { get; set; }

        /// <summary>
        /// Optional game version.
        /// </summary>
        public static string GameVersion { get; set; }

        /// <summary>
        /// Optional required engine version.
        /// Games must be run with the same version of the engine their package file was generated with.
        /// </summary>
        public static string EngineVersion { get; set; }

        /// <summary>
        /// Allow binaries to be included in the generated package.
        /// </summary>
        public static bool AllowBinaries { get; set; }
        
        /// <summary>
        /// If extract mode is on, specifies the input file to extract.
        /// </summary>
        public static string InFile { get; set; }

        /// <summary>
        /// If extract made is on, specifies the output folder to extract.
        /// </summary>
        public static string OutFolder { get; set; }

        /// <summary>
        /// The compression mode of the generated package.
        /// </summary>
        public static PackageFileCompressionMode CompressionMode { get; set; }

        static CommandLine()
        {
            CompressionMode = PackageFileCompressionMode.XOR;
        }

        public static bool Parse(string[] args)
        {
            try
            {
                if (args.Length < 4) return false;

                for (int curArg = 0; curArg < args.Length; curArg++)
                {
                    string thisArg = args[curArg];
                    string nextArg = null;

                    if (args.Length - curArg > 1) nextArg = args[curArg + 1];

                    // case-insensitive
                    thisArg = thisArg.ToLower();

                    switch (thisArg)
                    {
                        case "-infile":
                            InFile = nextArg;
                            continue;
                        case "-infolder":
                            InFolder = nextArg;
                            continue;
                        case "-outfile":
                            OutFile = nextArg;
                            continue;
                        case "-outfolder":
                            OutFolder = nextArg;
                            continue;
                        case "-name":
                        case "-gamename":
                            Name = nextArg;
                            continue;
                        case "-gameversion":
                            GameVersion = nextArg;
                            continue;
                        case "-engineversion":
                            EngineVersion = nextArg;
                            continue;
                        case "-allowbinaries":
                            AllowBinaries = true;
                            continue;
                        case "-compressionmode":
                            CompressionMode = (PackageFileCompressionMode)Enum.Parse(typeof(PackageFileCompressionMode), nextArg);
                            continue;
                    }

                }

                if (Name == null) Name = "Game name here";
                if (GameVersion == null) GameVersion = "1.0";
                // temporary version
                if (EngineVersion == null) EngineVersion = "1.0.138";

                //bad code
                if (InFile == null)
                {
                    if (InFolder == null
                        || OutFile == null)
                    {
                        NCLogging.Log("Error: If -infolder is specified, -outfile must be specified!", ConsoleColor.Red, false, false);
                        return false;
                    }

                    if (!OutFile.Contains(".wad")) NCLogging.Log("Warning: By convention, the package file for your game should have a .wad extension!", ConsoleColor.Yellow, false, false);
                }
                else
                {
                    if (!InFile.Contains(".wad")) NCLogging.Log("Warning: By convention, the package file for your game should have a .wad extension!", ConsoleColor.Yellow, false, false);

                    if (OutFolder == null) OutFolder = ".";

                    if (!Directory.Exists(OutFolder)) Directory.CreateDirectory(OutFolder);
                }

                // rough ending of the bad code

                return true;
            }
            catch (Exception err)
            {
                NCLogging.Log($"An error occurred while parsing arguments: {err.Message}");
                return false;
            }
        }

        public static void ShowHelp()
        {
            NCLogging.Log("MakePackage -infolder [input folder] -outfile [output folder] [args...]\n" +
                "MakePackage -infile [file] -outfolder [output folder]\n" +
                "Makes a Lightning package (WAD) file.\n\n" +
                "Required arguments:\n\n" +
                "-infolder: Input folder to generate the package file from.\n" +
                "-outfile: Output file for the package file. Should be .wad.\n" +
                "OR\n" +
                "-extract: Extract mode\n" +
                "-infile: Input file to extract the package file from\n" +
                "-outfolder: Output folder for the contents of the package file (Default value: current directory)\n" +
                "Optional arguments:\n\n" +
                "-gamename [-name]: Optional game name.\n" +
                "-gameversion: Optional game version.\n" +
                "-engineversion: Optional engine version.\n" +
                "-allowbinaries: Allow binaries in the WAD file. The default value is false.\n" +
                "-compressionmode: Final compression mode of the WAD file.", ConsoleColor.White, false, false);
        }
    }
}
