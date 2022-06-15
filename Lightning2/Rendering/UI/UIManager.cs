using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System.Collections.Generic;
using System.Numerics; 

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
            NCLogging.Log($"Creating new UIGadget::{uiElement.GetType().Name}");
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

        public static void MousePressed(SDL_MouseButton mouseButton, Vector2 position)
        {
            foreach (UIGadget uiElement in UIElements)
            {
                if (AABB.Intersects(uiElement, position))
                {
                    uiElement.OnMousePressed(mouseButton, position);
                }
            }
        }

        public static void MouseReleased(SDL_MouseButton mouseButton, Vector2 position)
        {
            foreach (UIGadget uiElement in UIElements)
            {
                if (uiElement.OnMouseReleased != null)
                {
                    if (AABB.Intersects(uiElement, position))
                    {
                        uiElement.OnMouseReleased(mouseButton, position);
                    }
                    
                }
            }
        }
    }
}
