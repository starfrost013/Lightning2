
namespace LightningGL
{
    internal static class InputMethodManager
    {
        internal static InputMethod? CurrentMethod { get; set; }

        internal static List<InputMethod> AvailableMethods { get; private set; }       

        static InputMethodManager()
        {
            AvailableMethods = new List<InputMethod>();
        }

        internal static void Init()
        {
            NCLogging.Log("Detecting input methods...");

            ControllerKeyboard keyboard = new();
            if (keyboard.DetectPresence()) AvailableMethods.Add(keyboard);
            ControllerMouse mouse = new();
            if (mouse.DetectPresence()) AvailableMethods.Add(mouse);
            ControllerDS4 ds4 = new();
            if (ds4.DetectPresence()) AvailableMethods.Add(ds4);

            // try generic xinput controller 
            if (!AvailableMethods.Contains(ds4))
            {
                ControllerXinput xinput = new();
                if (xinput.DetectPresence()) AvailableMethods.Add(xinput);
            }


        }
    }
}
