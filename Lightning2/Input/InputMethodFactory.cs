

namespace LightningGL
{
    internal static class InputMethodFactory
    {
        internal static InputMethod? GetInputMethod(InputMethods method)
        {
            return method switch
            {
                InputMethods.KeyboardMouse => new InputMethodKeyboardMouse(),
                InputMethods.Xinput => new InputMethodXinput(),
                _ => null,
            };
        }
    }
}
