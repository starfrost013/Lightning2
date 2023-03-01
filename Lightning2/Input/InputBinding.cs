
namespace LightningGL
{
    public class InputBinding
    {
        /// <summary>
        /// The name of the binding.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The binding value.
        /// This is internal to prevent bad programming practices by game developers.
        /// </summary>
        internal string Bind { get; set; }
        
        public InputBinding(string name, string bind)
        {
            Name = name;
            Bind = bind;
        }
    }
}
