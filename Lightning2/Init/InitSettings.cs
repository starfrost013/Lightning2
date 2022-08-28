using LightningPackager;

namespace LightningGL
{
    /// <summary>
    /// InitSettings
    /// 
    /// July 30, 2022 (modified August 10, 2022)
    /// 
    /// Stores initialisation settings.
    /// These are STRICTLY intended ONLY for debugging purposes.
    /// - It is my hope that these are the only command-line arguments needed, as I am trying to avoid them
    /// </summary>
    public class InitSettings
    {
        /// <summary>
        /// The package file containing the game files.
        /// </summary>
        public static string PackageFile { get; private set; }

        /// <summary>
        /// The folder to extract the package file specified by <see cref="PackageFile"/> to.
        /// </summary>
        public static string ContentFolder { get; private set; }

        /// <summary>
        /// Static constructor for InitSettings
        /// </summary>
        static InitSettings()
        {
            ContentFolder = "Content";
        }

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments passed to your application.</param>
        /// <returns>A boolean determining if command-line arguments were parsed successfully.</returns>
        internal static bool Parse(string[] args)
        {
            for (int curArg = 0; curArg < args.Length; curArg++)
            {
                string arg = args[curArg];
                string nextArg = null;

                if (args.Length - curArg > 1) nextArg = args[curArg + 1];

                arg = arg.ToLower();

                switch (arg)
                {
                    case "-packagefile":
                        PackageFile = nextArg;
                        continue;
                    case "-contentfolder":
                        ContentFolder = nextArg;
                        continue;
                }

            }

            return true;
        }
    }
}
