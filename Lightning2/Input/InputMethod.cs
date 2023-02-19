
namespace LightningGL
{
    /// <summary>
    /// Input Method API 
    /// 
    /// I am going to kill myself™ before this fucking game engine is done
    /// </summary>
    public abstract class InputMethod
    {
        public List<InputMethodBinding> Bindings { get; private set; }

        public InputMethod()
        {
            Bindings = new List<InputMethodBinding>();
        }

        internal virtual bool DetectPresence()
        {
            return false;
        }

        internal InputMethodBinding? GetBindingByBind(string bind)
        {
            foreach (InputMethodBinding binding in Bindings)
            {
                if (binding.Bind == bind) return binding;
            }

            return null;
        }
    }
}
