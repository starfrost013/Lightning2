
namespace LightningGL
{
    internal class UnbindCommand : ConsoleCommand
    {
        public override string Name => "unbind";
        public override bool Execute(params string[] parameters)
        {

            return true; 
        }

        public override string Description => "unbind [type] [binding]" +
            "Unbinds a control.\n" +
            "Parameters:\n" +
            "type (InputMethods): Xinput or KeyboardMouse: The type of binding to unbind\n" +
            "binding: The name of the binding to unbind.\n";
    }
}
