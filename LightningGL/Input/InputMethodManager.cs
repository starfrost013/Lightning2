namespace LightningGL
{
    internal static class InputMethodManager
    {
        internal static InputMethod? CurrentMethod { get; set; }

        internal static InputMethodKeyboardMouse KeyboardMouse { get; set; }

        internal static InputMethodXinput Xinput { get; set; }    

        internal static IniFile? InputIni { get; set; }

        static InputMethodManager()
        {
            KeyboardMouse = new();
            Xinput = new();
        }

        internal static void ScanAvailableInputMethods(bool reloadBindings = false)
        {
            if (CurrentMethod != null) CurrentMethod = null; 

            Logger.Log("Detecting input methods...");

            if (KeyboardMouse.DetectPresence()) CurrentMethod = KeyboardMouse;
            if (Xinput.DetectPresence()) CurrentMethod = Xinput;

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

                IniSection? sectionXinput = InputIni.GetSection("Xinput");
                IniSection? sectionKeyboardMouse = InputIni.GetSection("KeyboardMouse");

                if (sectionXinput != null) foreach (var value in sectionXinput.Values) Xinput.Bindings.Add(new(value.Key, value.Value));
                if (sectionKeyboardMouse != null) foreach (var value in sectionKeyboardMouse.Values) KeyboardMouse.Bindings.Add(new(value.Key, value.Value));
            }
        }

        internal static bool AddBinding(InputMethods section, string binding, string bind)
        {
            // should never be null, user can theoretically do this so not an assert
            
            if (InputIni == null)
            {
                Logger.LogError("InputMethodManager::AddBinding called before InputMethodManager::ScanForAvailableMethods!", 322, LoggerSeverity.FatalError);
                return false;
            }

            IniSection? iniSection = InputIni.GetSection(section.ToString());

            // case insensitive search so should never be null considering previous checks
            Debug.Assert(iniSection != null);

            // add to the local state
            switch (section)
            {
                case InputMethods.KeyboardMouse:
                    KeyboardMouse.Bindings.Add(new(binding, bind));
                    break;
                case InputMethods.Xinput:
                    Xinput.Bindings.Add(new(binding, bind));
                    break;

            }

            // add to the ini file
            iniSection.Values.Add(binding, bind);

            // save the INI file
            InputIni.Save(GlobalSettings.INPUTBINDINGS_PATH);

            return true;
        }

        internal static bool ModifyBinding(InputMethods section, string binding, string newBind)
        {
            if (InputIni == null)
            {
                Logger.LogError("InputMethodManager::AddBinding called before InputMethodManager::ScanForAvailableMethods!", 323, LoggerSeverity.FatalError);
                return false;
            }

            IniSection? iniSection = InputIni.GetSection(section.ToString());

            Debug.Assert(iniSection != null); 

            // change in local state

            switch (section)
            {
                case InputMethods.KeyboardMouse:
                    InputBinding? inputBindingKeyboardMouse = KeyboardMouse.GetBindingByName(binding);
                    
                    if (inputBindingKeyboardMouse == null)
                    {
                        Logger.LogError("Attempted to get invalid input binding for modification!", 324, LoggerSeverity.Error);
                        return false;
                    }

                    inputBindingKeyboardMouse.Bind = newBind;
                    break;
                case InputMethods.Xinput:
                    InputBinding? inputBindingXinput = Xinput.GetBindingByName(binding);

                    if (inputBindingXinput == null)
                    {
                        Logger.LogError("Attempted to get invalid input binding for modification!", 325, LoggerSeverity.Error);
                        return false;
                    }

                    inputBindingXinput.Bind = newBind;
                    break;
            }

            // change in ini file and save

            iniSection.Values[binding] = newBind;
            InputIni.Save(GlobalSettings.INPUTBINDINGS_PATH);
            return true; 
        }

        internal static bool RemoveBinding(InputMethods section, string binding)
        {
            if (InputIni == null)
            {
                Logger.LogError("InputMethodManager::AddBinding called before InputMethodManager::ScanForAvailableMethods!", 323, LoggerSeverity.FatalError);
                return false;
            }

            IniSection? iniSection = InputIni.GetSection(section.ToString());

            Debug.Assert(iniSection != null);

            // remove in local state

            switch (section)
            {
                case InputMethods.KeyboardMouse:
                    InputBinding? inputBindingKeyboardMouse = KeyboardMouse.GetBindingByName(binding);

                    if (inputBindingKeyboardMouse == null)
                    {
                        Logger.LogError("Attempted to get invalid input binding for modification!", 326, LoggerSeverity.Error);
                        return false;
                    }

                    KeyboardMouse.Bindings.Remove(inputBindingKeyboardMouse);
                    break;
                case InputMethods.Xinput:
                    InputBinding? inputBindingXinput = KeyboardMouse.GetBindingByName(binding);

                    if (inputBindingXinput == null)
                    {
                        Logger.LogError("Attempted to get invalid input binding for modification!", 327, LoggerSeverity.Error);
                        return false;
                    }

                    KeyboardMouse.Bindings.Remove(inputBindingXinput);
                    break;
            }

            // remove from INI file and save
            iniSection.Values.Remove(binding);
            InputIni.Save(GlobalSettings.INPUTBINDINGS_PATH);

            return true;
        }
    }
}
