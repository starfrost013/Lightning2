
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

            return true; 
        }

        public override string Description => "unbind [type] [binding]" +
            "Unbinds a control.\n" +
            "Parameters:\n" +
            "type (InputMethods): Xinput or KeyboardMouse: The type of binding to unbind\n" +
            "binding: The name of the binding to unbind.\n";
    }
}
