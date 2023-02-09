

namespace LightningGL
{
    internal static class InputMethodFactory
    {
        internal static InputMethod? GetInputMethod(InputMethods method)
        {
            switch (method)
            {
                case InputMethods.KeyboardMouse:
                    return new ControllerKeyboardMouse();
                case InputMethods.DS4:
                    return new ControllerDS4();
                case InputMethods.Xinput:
                    return new ControllerXinput();
            }
            return null;

        }
    }
}
