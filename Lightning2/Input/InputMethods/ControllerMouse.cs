
namespace LightningGL
{
    internal class ControllerMouse : InputMethod
    {
        internal override bool DetectPresence()
        {
            NCLogging.Log("Detecting mouse...");
            // SDL has no mouse detection code, no point writing it.
            return true; 
        }
    }
}
