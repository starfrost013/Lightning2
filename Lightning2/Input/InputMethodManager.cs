
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

            ControllerKeyboardMouse keyboard = new();
            if (keyboard.DetectPresence())
            {
                AvailableMethods.Add(keyboard);
                CurrentMethod = keyboard;
            }
            ControllerDS4 ds4 = new();

            if (ds4.DetectPresence())
            {
                AvailableMethods.Add(ds4);
                CurrentMethod = ds4;
            }

            // try generic xinput controller 
            if (CurrentMethod != ds4)
            {
                ControllerXinput xinput = new();
                if (xinput.DetectPresence()) AvailableMethods.Add(xinput);
                CurrentMethod = xinput;
            }
        }

        internal static void OnDisconnect()
        {
            // reset to default for now
            CurrentMethod = InputMethodFactory.GetInputMethod(GlobalSettings.GeneralDefaultInputMethod);
        }
    }
}
