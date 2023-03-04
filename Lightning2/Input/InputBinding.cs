
namespace LightningGL
{
    public class InputBinding
    {
        private string _name;

        /// <summary>
        /// The name of the binding.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {   
                // while i would prefer if people used string.Equals with CultureInfo.InvariantCultureIgnoreCase,
                // people don't and therefore doing this is more "intuitive"
                _name = value.ToUpperInvariant();
            }
        }

        /// <summary>
        /// Backing field for <see cref="Bind"/>.
        /// </summary>
        private string _bind;

        /// <summary>
        /// The binding value.
        /// This is internal to prevent bad programming practices by game developers.
        /// </summary>
        internal string Bind
        {
            get
            {
                return _bind;
            }
            set
            {
                // while i would prefer if people used string.Equals with CultureInfo.InvariantCultureIgnoreCase,
                // people don't and therefore doing this is more "intuitive"
                _bind = value.ToUpperInvariant(); 
            }
        }
        
        public InputBinding(string name, string bind)
        {
            Name = name;
            Bind = bind;
            // hack for weird nullable processing issue causing warning CS8618,
            // even if the setter of the property automatically sets its backing field

            _bind = Bind.ToUpperInvariant();
            _name = Name.ToUpperInvariant();
        }
    }
}
