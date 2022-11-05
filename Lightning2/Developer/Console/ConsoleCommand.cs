
namespace LightningGL
{
    /// <summary>
    /// ConsoleCommand
    /// 
    /// Abstract base class for console commands.
    /// </summary>
    public abstract class ConsoleCommand
    {
        /// <summary>
        /// The name of the console command.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// The description of the console command.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Executes the console command.
        /// </summary>
        /// <param name="cRenderer">The renderer used for execution..</param>
        public abstract void Execute(Renderer cRenderer);
    }
}
