
namespace LightningBase
{
    [Flags]
    /// <summary>
    /// DebugState
    /// 
    /// Internal state for toggling the state of the debug engine.
    /// </summary>
    public enum DebugState
    {
        /// <summary>
        /// Not loaded indicator for <see cref="GlobalSettings"/>.
        /// </summary>
        NotLoaded = 0x0,

        /// <summary>
        /// No debugging functionality enabled.
        /// </summary>
        None = 0x1,

        /// <summary>
        /// The debug console is enabled.
        /// </summary>
        DebugConsole = 0x2,

        /// <summary>
        /// The debug display (DebugKey) is enabled.
        /// </summary>
        DebugViewer = 0x4,

        /// <summary>
        /// Both debugging functions are enabled.
        /// </summary>
        Both = (DebugConsole | DebugViewer),
    }
}
