namespace LightningGL
{
    internal static class InputMethodManager
    {
        internal static InputMethod? CurrentMethod { get; set; }

        internal static List<InputMethod> AvailableMethods { get; private set; }       

        internal static IniFile? InputIni { get; set; }

        static InputMethodManager()
        {
            AvailableMethods = new List<InputMethod>();
        }

        internal static void ScanAvailableInputMethods(bool reloadBindings = false)
        {
            // reset
            if (AvailableMethods.Count > 0) AvailableMethods.Clear();
            if (CurrentMethod != null) CurrentMethod = null; 

            Logger.Log("Detecting input methods...");

            InputMethodKeyboardMouse keyboardMouse = new();

            if (keyboardMouse.DetectPresence())
            {
                AvailableMethods.Add(keyboardMouse);
                CurrentMethod = keyboardMouse;
            }

            InputMethodXinput xinput = new();

            if (xinput.DetectPresence())
            {
                AvailableMethods.Add(xinput);
                CurrentMethod = xinput;
            }

            if (reloadBindings)
            {
                Logger.Log("Loading input bindings from InputBindings.ini...");

                // need a better trimming-compatible solution for this as we add more
                InputIni = IniFile.Parse(GlobalSettings.INPUTBINDINGS_PATH);

                if (InputIni == null)
                {
                    Logger.LogError("Cannot find InputBindings.ini!", 305, LoggerSeverity.Warning, null, true);
                    return;
                }

                IniSection? sectionDs4 = InputIni.GetSection("DS4");
                IniSection? sectionXinput = InputIni.GetSection("Xinput");
                IniSection? sectionKeyboardMouse = InputIni.GetSection("KeyboardMouse");

                if (sectionDs4 != null) foreach (var value in sectionDs4.Values) ds4.Bindings.Add(new(value.Key, value.Value));
                if (sectionXinput != null) foreach (var value in sectionXinput.Values) xinput.Bindings.Add(new(value.Key, value.Value));
                if (sectionKeyboardMouse != null) foreach (var value in sectionKeyboardMouse.Values) keyboardMouse.Bindings.Add(new(value.Key, value.Value));
            }
        }

        internal static void AddBinding(InputMethods section, string binding, string bind)
        {
            // should never be null, user can theoretically do this so not an assert
            
            if (InputIni == null)
            {
                Logger.LogError("InputMethodManager::AddBinding called before InputMethodManager::ScanForAvailableMethods!", 322, LoggerSeverity.FatalError);
                return;
            }

            IniSection? iniSection = InputIni.GetSection(section.ToString());

            // case insensitive search so should never be null considering previous checks
            Debug.Assert(iniSection != null);

            // add to the local state


            // add to the ini state
            iniSection.Values.Add(binding, bind);

            // save the INI (maybe move this to shutdown!!)
            InputIni.Save(GlobalSettings.INPUTBINDINGS_PATH);
        }

        internal static void ModifyBinding(InputMethods section, string binding, string newBind)
        {

        }
    }
}
