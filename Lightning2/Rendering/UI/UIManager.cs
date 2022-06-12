using System.Collections.Generic;

namespace LightningGL
{
    /// <summary>
    /// UIManager
    /// 
    /// May 15, 2022
    /// 
    /// A simple UI manager - all UI is on one layer, there is no hierarchy
    /// </summary>
    public static class UIManager
    {
        private static List<UIElement> UIElements { get; set; }

        static UIManager()
        {
            UIElements = new List<UIElement>();
        }

        public static void AddUiElement(UIElement uiElement)
        {
            UIElements.Add(uiElement);
        }

        internal static void Render(Window cWindow)
        {
            foreach (UIElement uiElement in UIElements)
            {
                if (uiElement.OnRender != null)
                {
                    uiElement.OnRender(cWindow);
                }
            }
        }

        internal static void Shutdown(Window cWindow)
        {
            foreach (UIElement uiElement in UIElements)
            {
                if (uiElement.OnShutdown != null)
                {
                    uiElement.OnShutdown(cWindow);
                }
            }
        }
    }
}
