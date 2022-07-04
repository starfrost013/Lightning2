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
        private static List<Gadget> UIElements { get; set; }

        static UIManager()
        {
            UIElements = new List<Gadget>();
        }

        public static void AddElement(Gadget uiElement)
        {
            NCLogging.Log($"Creating new Gadget::{uiElement.GetType().Name}");
            UIElements.Add(uiElement);
        }

        internal static void Render(Window cWindow)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnRender != null)
                {
                    uiElement.OnRender(cWindow);
                }
            }
        }

        internal static void Shutdown(Window cWindow)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnShutdown != null)
                {
                    uiElement.OnShutdown(cWindow);
                }
            }
        }

        public static void MousePressed(Window cWindow, SDL_MouseButton mouseButton, Vector2 position)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            Vector2 cameraPosition = position;

            if (currentCamera != null)
            {
                cameraPosition = new Vector2
                    (position.X - currentCamera.Position.X,
                    position.Y - currentCamera.Position.Y);
            }

            foreach (Gadget uiElement in UIElements)
            {
                if (AABB.Intersects(uiElement, cameraPosition)
                    && uiElement.OnMousePressed != null)
                {
                    uiElement.OnMousePressed(mouseButton, cameraPosition);
                }
            }
        }

        public static void MouseReleased(Window cWindow, SDL_MouseButton mouseButton, Vector2 position)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            Vector2 cameraPosition = position;

            if (currentCamera != null)
            {
                cameraPosition = new Vector2
                    (position.X - currentCamera.Position.X,
                    position.Y - currentCamera.Position.Y);
            }

            foreach (Gadget uiElement in UIElements)
            {
                if (AABB.Intersects(uiElement, cameraPosition)
                    && uiElement.OnMouseReleased != null)
                {
                    uiElement.OnMouseReleased(mouseButton, cameraPosition);
                }
            }
        }

        public static void MouseEnter(Window cWindow)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseEnter != null)
                {
                    uiElement.OnMouseEnter();
                }
            }
        }
        public static void MouseLeave(Window cWindow)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseLeave != null)
                {
                    uiElement.OnMouseLeave();
                }
            }
        }

        public static void MouseMove(Window cWindow, Vector2 position, Vector2 velocity, SDL_MouseButton mouseButton)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseMove != null) // this one is passed regardless of intersection for things like button highlighting
                {
                    uiElement.OnMouseMove(position, velocity, mouseButton);
                }
            }
        }
    }
}