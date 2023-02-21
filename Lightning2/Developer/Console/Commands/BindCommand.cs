
namespace LightningGL
{
    internal class BindCommand : ConsoleCommand
    {
        public override string Name => "bind";
        public override bool Execute(params string[] parameters)
        {

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
