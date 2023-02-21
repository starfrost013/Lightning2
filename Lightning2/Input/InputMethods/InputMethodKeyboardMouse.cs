

namespace LightningGL
{
    /// <summary>
    /// Input method for the keyboard.
    /// </summary>
    internal class InputMethodKeyboardMouse : InputMethod
    {
        internal override bool DetectPresence()
        {
            NCLogging.Log("Detecting keyboard...");
            return true;
        }

    }
}
