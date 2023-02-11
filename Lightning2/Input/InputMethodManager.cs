
namespace LightningGL
{
    internal static class InputMethodManager
    {
        internal static InputMethod? CurrentMethod { get; set; }

        internal static List<InputMethod> AvailableMethods { get; private set; }       

        private static NCINIFile? InputIni { get; set; }

        static InputMethodManager()
        {
            AvailableMethods = new List<InputMethod>();
        }

        internal static void Init()
        {
            NCLogging.Log("Detecting input methods...");

            ControllerKeyboardMouse keyboardMouse = new();

            if (keyboardMouse.DetectPresence())
            {
                AvailableMethods.Add(keyboardMouse);
                CurrentMethod = keyboardMouse;
            }
            
            ControllerDS4 ds4 = new();

            if (ds4.DetectPresence())
            {
                AvailableMethods.Add(ds4);
                CurrentMethod = ds4;
            }

            ControllerXinput xinput = new();

            if (xinput.DetectPresence())
            {
                AvailableMethods.Add(xinput);

                // don't 'overwrite' a DS4 controller with a generic xinput controller
                // (for now - we will fix this when we add more controllers)
                if (CurrentMethod != ds4) CurrentMethod = xinput;
            }

            NCLogging.Log("Loading input bindings from InputBindings.ini...");

            // need a better trimming-compatible solution for this as we add more
            InputIni = NCINIFile.Parse(GlobalSettings.INPUTBINDINGS_PATH);

            if (InputIni == null)
            {
                NCLogging.LogError("Cannot find InputBindings.ini!", 305, NCLoggingSeverity.Warning, null, true);
                return; 
            }

            NCINIFileSection? sectionDs4 = InputIni.GetSection("DS4");
            NCINIFileSection? sectionXinput = InputIni.GetSection("Xinput");
            NCINIFileSection? sectionKeyboardMouse = InputIni.GetSection("KeyboardMouse");

            if (sectionDs4 != null) foreach (var value in sectionDs4.Values) ds4.Bindings.Add(new(value.Key, value.Value));
            if (sectionXinput != null) foreach (var value in sectionXinput.Values) xinput.Bindings.Add(new(value.Key, value.Value));
            if (sectionKeyboardMouse != null) foreach (var value in sectionKeyboardMouse.Values) keyboardMouse.Bindings.Add(new(value.Key, value.Value));
        }

        internal static void OnDisconnect()
        {
            // reset to default for now
            CurrentMethod = InputMethodFactory.GetInputMethod(GlobalSettings.GeneralDefaultInputMethod);
        }
    }
}
