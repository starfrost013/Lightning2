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

        public static bool Parse(string[] args)
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
                    case "-infolder":
                        InFolder = nextArg;
                        continue;
                    case "-outfile":
                        OutFile = nextArg;
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
                }
               
            }

            if (Name == null) Name = "Game name here";
            if (GameVersion == null) GameVersion = "1.0";
            // temporary version
            if (EngineVersion == null) EngineVersion = "1.0.138";

            if (InFolder == null
                   || OutFile == null)
            {
                NCLogging.Log("Error: -infolder and -outfile must both be specified!", ConsoleColor.Red, false, false);
                return false;
            }

            if (!OutFile.Contains(".wad")) NCLogging.Log("Warning: By convention, the package file for your game should have a .wad extension!", ConsoleColor.Yellow, false, false);

            return true; 
        }

        public static void ShowHelp()
        {
            NCLogging.Log("MakePackage -infolder [input folder] -outfile [output folder] [args...]\n" +
                "Makes a Lightning package file.\n\n" +
                "Required arguments:\n\n" +
                "-infolder: Input folder to generate the package file from.\n" +
                "-outfile: Output file for the package file. Should be .wad.\n\n" +
                "Optional arguments:\n\n" +
                "-gamename [-name]: Optional game name.\n" +
                "-gameversion: Optional game version.\n" +
                "-engineversion: Optional engine version.\n", ConsoleColor.White, false, false);
        }
    }
}
