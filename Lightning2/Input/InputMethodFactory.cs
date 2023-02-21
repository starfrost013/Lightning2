

namespace LightningGL
{
    internal static class InputMethodFactory
    {
        internal static InputMethod? GetInputMethod(InputMethods method)
        {
            switch (method)
            {
                case InputMethods.KeyboardMouse:
                    return new InputMethodKeyboardMouse();
                case InputMethods.DS4:
                    return new InputMethodDS4();
                case InputMethods.Xinput:
                    return new InputMethodXinput();
            }
            return null;

        }
    }
}
