
namespace LightningNetwork
{
    /// <summary>
    /// LNetCommand
    /// 
    /// Defines an LNet command. 
    /// </summary>
    public abstract class LNetCommand
    {
        public abstract string Name { get; }

        /// <summary>
        /// ID of this command. Should always be equal to this command's value in <see cref="LNetCommandTypes"/>.
        /// </summary>
        public abstract byte Id { get; }

        public abstract void OnSend();
        public abstract void OnReceive();
        public virtual byte[] ToByteArray() => new byte[1] { Id };  
    }
}
