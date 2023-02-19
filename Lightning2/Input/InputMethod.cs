
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

        internal InputBinding? GetBindingByBind(string bind)
        {
            foreach (InputBinding binding in Bindings)
            {
                if (binding.Bind == bind) return binding;
            }

            return null;
        }
    }
}
