using NuCore.Utilities;
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
        private static List<UIGadget> UIElements { get; set; }

        static UIManager()
        {
            UIElements = new List<UIGadget>();
        }

        public static void AddElement(UIGadget uiElement)
        {
            NCLogging.Log($"Creating UIElement: {uiElement.GetType().Name}");
            UIElements.Add(uiElement);
        }

        internal static void Render(Window cWindow)
        {
            foreach (UIGadget uiElement in UIElements)
            {
                if (uiElement.OnRender != null)
                {
                    uiElement.OnRender(cWindow);
                }
            }
        }

        internal static void Shutdown(Window cWindow)
        {
            foreach (UIGadget uiElement in UIElements)
            {
                if (uiElement.OnShutdown != null)
                {
                    uiElement.OnShutdown(cWindow);
                }
            }
        }
    }
}
