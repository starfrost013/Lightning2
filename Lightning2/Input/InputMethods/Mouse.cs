
namespace LightningGL
{
    internal class Mouse : InputMethod
    {
        internal override bool DetectPresence()
        {
            // SDL has no mouse detection code, no point writing it.
            return true; 
        }
    }
}
