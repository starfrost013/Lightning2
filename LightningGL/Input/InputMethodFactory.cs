

namespace LightningGL
{
    internal static class InputMethodFactory
    {
        internal static InputMethod? GetInputMethod(InputMethods method)
        {
            return method switch
            {
                InputMethods.KeyboardMouse => InputMethodManager.KeyboardMouse,
                InputMethods.Xinput => InputMethodManager.Xinput,
                _ => null,
            };
        }
    }
}
