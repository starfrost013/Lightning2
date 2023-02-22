namespace LightningGL
{
    internal class BindCommand : ConsoleCommand
    {
        public override string Name => "bind";

        private const string LOGGING_PREFIX = "Command: bind";
            
        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length != 3)
            {
                Logger.Log(LOGGING_PREFIX, "Invalid number of parameters!");
                return false;
            }

            if (!Enum.TryParse(parameters[0], true, out InputMethods inputMethod))
            {
                Logger.Log(LOGGING_PREFIX, "Invalid input method! (valid methods: KeyboardMouse, DS4, Xinput)");
                return false;
            }

            InputMethod? inputMethodInstance = InputMethodFactory.GetInputMethod(inputMethod);

            // this should never be null, considering previous checks
            Debug.Assert(inputMethodInstance != null
                && InputMethodManager.InputIni != null);

            ref string bindingName = ref parameters[1];

            foreach (InputBinding inputBindings in inputMethodInstance.Bindings)
            {
                // the binding already exists
                if (inputBindings.Name == bindingName)
                {

                }

            }

            string bind = parameters[2];

            Logger.Log($"Creating binding {bindingName} ({bind}), section: {inputMethod}...");
            // case: binding doesn't exist, create it
            InputMethodManager.AddBinding(inputMethod, bindingName, bind);

            return true; 
        }

        public override string Description => "bind [type] [binding] [bind]\n" +
            "Binds a control.\n" +
            "Parameters:\n" +
            "type (InputMethods): DS4, Xinput, or KeyboardMouse: The type of binding to use\n" +
            "binding: The name of the binding.\n" +
            "bind: The binding to bind.";
    }
}
