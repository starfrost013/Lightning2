
namespace LightningGL
{
    internal class ShutdownCommand : ConsoleCommand
    {
        public override string Name => "shutdown";
        public override bool Execute(params string[] parameters)
        {
            Shutdown();
            return true;
        }

        public override string Description => "Shutdown.\nShut down the engine.";
    }
}
