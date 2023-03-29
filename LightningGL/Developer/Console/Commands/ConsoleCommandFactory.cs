
namespace LightningGL
{
    internal static class ConsoleCommandFactory
    {
        internal static ConsoleCommand? GetCommand(ConsoleCommands commands)
        {
            switch (commands)
            {
                case ConsoleCommands.SetGVar:
                    return new SetGlobalSettingCommand();
                case ConsoleCommands.SetLVar:
                    return new SetLocalSettingCommand();
                case ConsoleCommands.SetCamera:
                    return new SetCameraCommand();
                case ConsoleCommands.SetScene:
                    return new SetSceneCommand();
                case ConsoleCommands.Shutdown:
                    return new ShutdownCommand();
                case ConsoleCommands.Bind:
                    return new BindCommand();
                case ConsoleCommands.Unbind:
                    return new UnbindCommand(); 
                default:
                    return null;
            }
        }
    }
}
