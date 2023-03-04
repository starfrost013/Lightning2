
namespace LightningGL
{
    /// <summary>
    /// Input Method API 
    /// 
    /// I am going to kill myself™ before this fucking game engine is done
    /// </summary>
    public abstract class InputMethod
    {
        public List<InputBinding> Bindings { get; private set; }

        public InputMethod()
        {
            Bindings = new List<InputBinding>();
        }

        internal virtual bool DetectPresence()
        {
            return false;
        }

        internal InputBinding? GetBindingByName(string name)
        {
            foreach (InputBinding binding in Bindings)
            {
                if (string.Equals(binding.Name, name, StringComparison.InvariantCultureIgnoreCase)) return binding;
            }

            return null;
        }

        internal InputBinding? GetBindingByBind(string bind)
        {
            foreach (InputBinding binding in Bindings)
            {
                if (string.Equals(binding.Bind, bind, StringComparison.InvariantCultureIgnoreCase)) return binding;
            }

            return null;
        }
    }
}
