
namespace LightningGL
{
    internal class UnbindCommand : ConsoleCommand
    {
        public override string Name => "unbind";

        private const string LOGGING_PREFIX = "Command: unbind";

        public override bool Execute(params string[] parameters)
        {
            if (parameters.Length != 2)
            {
                Logger.Log(LOGGING_PREFIX, "Incorrect number of parameters");
                return false; 
            }

            if (!Enum.TryParse(parameters[0], true, out InputMethods inputMethod))
            {
                Logger.Log(LOGGING_PREFIX, "Invalid input method! (valid methods: KeyboardMouse, Xinput)");
                return false;
            }

            InputMethod? inputMethodInstance = InputMethodFactory.GetInputMethod(inputMethod);

            // this should never be null, considering previous checks
            Debug.Assert(inputMethodInstance != null
                && InputMethodManager.InputIni != null);

            ref string bindingName = ref parameters[1];

            Logger.Log($"Removing binding {bindingName}, section: {inputMethod}...");

            // case: binding doesn't exist, create it
            return InputMethodManager.RemoveBinding(inputMethod, bindingName);
        }

        public override string Description => "unbind [type] [binding]" +
            "Unbinds a control.\n" +
            "Parameters:\n" +
            "type (InputMethods): Xinput or KeyboardMouse: The type of binding to unbind\n" +
            "binding: The name of the binding to unbind.\n";
    }
}
