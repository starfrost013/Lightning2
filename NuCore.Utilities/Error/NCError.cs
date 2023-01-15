using System.Text;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCException
    /// 
    /// February 1, 2022
    /// 
    /// Defines a NuCore exception
    /// </summary>
    public static class NCError
    {
        /// <summary>
        /// Display an error box.
        /// </summary>
        /// <param name="description">A description of the error.</param>
        /// <param name="id">The ID of the error.</param>
        /// <param name="cause">An optional more detailed error cause - usually the code condition that caused it</param>
        /// <param name="exceptionSeverity">The severity of the exception - see <see cref="NCErrorSeverity"/></param>
        /// <param name="baseException">The .NET exception that caused the error, if present.</param>
        /// <param name="dontShowMessageBox">Determines if a message box was shown or not</param>
        public static void ShowErrorBox(string description, int id, NCErrorSeverity exceptionSeverity = NCErrorSeverity.Message,
            Exception? baseException = null, bool dontShowMessageBox = false)
        {
            StringBuilder stringBuilder = new();

            if (description != null) stringBuilder.Append($"{description}");
            if (exceptionSeverity > NCErrorSeverity.Message) stringBuilder.Append($" [{id}]");

            if (baseException != null) stringBuilder.Append($"\n\nError Information:\n{baseException}");

            string errorString = stringBuilder.ToString();

            NCLogging.Log($"{exceptionSeverity}:\n{errorString}", exceptionSeverity);

            // display message box
            if (NCAssembly.NCLightningExists
                && !dontShowMessageBox)
            {
                Debug.Assert(NCAssembly.NCLightningAssembly != null);
                Type? lightningUtilName = NCAssembly.NCLightningAssembly.GetType(NCAssembly.LIGHTNING_UTILITIES_PRESET_NAME, false, true);

                if (lightningUtilName == null)
                {
                    NCLogging.Log("Failed to load NCMessageBox type through reflection (ignoring)", ConsoleColor.Yellow);
                    return; 
                }

                MethodBase? msgBoxOk = lightningUtilName.GetMethod("MessageBoxOK");

                if (msgBoxOk == null)
                {
                    NCLogging.Log("Failed to display error box (ignoring)", ConsoleColor.Yellow);
                    return;
                }

                switch (exceptionSeverity)
                {
                    case NCErrorSeverity.Message:
                        msgBoxOk.Invoke(null, new object[]
                        { "Information", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION });
                        break;
                    case NCErrorSeverity.Warning:
                        msgBoxOk.Invoke(null, new object[]
                        { "Warning", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING });
                        break;
                    case NCErrorSeverity.Error:
                        msgBoxOk.Invoke(null, new object[]
                        { "Error", errorString, SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;
                    case NCErrorSeverity.FatalError:
                        msgBoxOk.Invoke(null, new object[]
                            { "Fatal Error", $"A fatal error has occurred:\n\n{errorString}\n\n" +
                                $"The Lightning Game Engine-based application you are running must exit. We are sorry for the inconvenience.",
                                SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR });
                        break;

                }
            }

            if (exceptionSeverity == NCErrorSeverity.FatalError) Environment.Exit(id);
        }
    }
}

/// <summary>
/// Define this here.
/// 
/// This is to reduce time spent in reflection which is very slow.
/// </summary>
[Flags]
internal enum SDL_MessageBoxFlags : uint
{
    SDL_MESSAGEBOX_ERROR = 0x00000010,

    SDL_MESSAGEBOX_WARNING = 0x00000020,

    SDL_MESSAGEBOX_INFORMATION = 0x00000040
}