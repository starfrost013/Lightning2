using static NuCore.SDL2.SDL;
using NuCore.Utilities;
using System.Collections.Generic;
using System.Numerics; 

namespace LightningGL
{
    /// <summary>
    /// UIManager
    /// 
    /// May 15, 2022 (modified July 31, 2022)
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

        /// <summary>
        /// Adds a <see cref="Gadget"/> to the UI manager.
        /// </summary>
        /// <param name="uiElement"></param>
        public static void AddElement(Gadget uiElement)
        {
            NCLogging.Log($"Creating new Gadget::{uiElement.GetType().Name}");
            UIElements.Add(uiElement);
        }

        internal static void Render(Window cWindow)
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.Size == default) _ = new NCException($"Attempted to draw a UI element with no size, you will not see it!", 122, "Gadget::Size = (0,0)!", NCExceptionSeverity.Warning, null, true);
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

        public static void MousePressed(Window cWindow, MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Gadget uiElement in UIElements)
            {
                bool intersects = AABB.Intersects(uiElement, button.Position);

                // check if it is focused...
                uiElement.Focused = intersects;

                if (intersects
                    && uiElement.OnMousePressed != null)
                {
                    uiElement.OnMousePressed(button);
                }
            }
        }

        public static void MouseReleased(Window cWindow, MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Gadget uiElement in UIElements)
            {
                bool intersects = AABB.Intersects(uiElement, button.Position);

                // check if it is focused...
                uiElement.Focused = intersects;

                if (intersects
                    && uiElement.OnMouseReleased != null)
                {
                    uiElement.OnMouseReleased(button);
                }
            }
        }

        public static void MouseEnter()
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseEnter != null)
                {
                    uiElement.OnMouseEnter();
                }
            }
        }

        public static void MouseLeave()
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseLeave != null)
                {
                    uiElement.OnMouseLeave();
                }
            }
        }

        public static void FocusGained()
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnFocusGained != null)
                {
                    uiElement.OnFocusGained();
                }
            }
        }

        public static void FocusLost()
        {
            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnFocusLost != null)
                {
                    uiElement.OnFocusLost();
                }
            }
        }

        public static void MouseMove(Window cWindow, MouseButton button)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cWindow.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Gadget uiElement in UIElements)
            {
                if (uiElement.OnMouseMove != null) // this one is passed regardless of intersection for things like button highlighting
                {
                    uiElement.OnMouseMove(button);
                }
            }
        }

        public static void KeyPressed(Key key)
        {
            foreach (Gadget uiElement in UIElements)
            {
                // check if the UI element is focused.
                if (uiElement.Focused
                    && uiElement.OnKeyPressed != null)
                {
                    uiElement.OnKeyPressed(key);
                }
            }
        }

        public static void KeyReleased(Key key)
        {
            foreach (Gadget uiElement in UIElements)
            {
                // check if the UI element is focused.
                if (uiElement.Focused
                    && uiElement.OnKeyReleased != null)
                {
                    uiElement.OnKeyReleased(key);
                }
            }
        }
    }
}