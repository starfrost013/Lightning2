namespace LightningGL
{
    /// <summary>
    /// UIManager
    /// 
    /// May 15, 2022 (modified July 31, 2022)
    /// 
    /// A simple UI manager - all UI is on one layer, there is no hierarchy
    /// </summary>
    public class UIAssetManager : AssetManager<Gadget>
    {
        /// <summary>
        /// Adds a <see cref="Gadget"/> to the UI manager.
        /// </summary>
        /// <param name="gadget">The <see cref=""/></param>
        public override void AddAsset(Window cWindow, Gadget gadget)
        {
            NCLogging.Log($"Creating new Gadget::{gadget.GetType().Name}");
            AssetList.Add(gadget);
        }

        public override void RemoveAsset(Window cWindow, Gadget gadget)
        {
            if (!AssetList.Contains(gadget)) _ = new NCException($"Attempted to remove a gadget of type ({gadget.GetType().Name} that is not in the UI Manager - you must add it first!", 135, "Called UIManager::RemoveElement with a gadget property that does not correspond to a Gadget loaded by the UI Manager!", NCExceptionSeverity.Warning);
            NCLogging.Log($"Removing A Gadget::{gadget.GetType().Name} from UIManager");
            AssetList.Remove(gadget);
        }

        /// <summary>
        /// Renders all UI elements.
        /// </summary>
        /// <param name="cWindow">The UI element to render.</param>
        internal void Render(Window cWindow)
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.Size == default) _ = new NCException($"Attempted to draw a gadget with no size, you will not see it!", 122, "Gadget::Size = (0,0)!", NCExceptionSeverity.Warning, null, true);
                if (uiElement.OnRender != null)
                {
                    uiElement.OnRender(cWindow);
                }
            }
        }

        internal void Shutdown(Window cWindow)
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnShutdown != null)
                {
                    uiElement.OnShutdown(cWindow);
                }
            }
        }

        internal void MousePressed(Window cWindow, MouseButton button)
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

            foreach (Gadget uiElement in AssetList)
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

        internal void MouseReleased(Window cWindow, MouseButton button)
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

            foreach (Gadget uiElement in AssetList)
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

        internal void MouseEnter()
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnMouseEnter != null)
                {
                    uiElement.OnMouseEnter();
                }
            }
        }

        internal void MouseLeave()
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnMouseLeave != null)
                {
                    uiElement.OnMouseLeave();
                }
            }
        }

        internal void FocusGained()
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnFocusGained != null)
                {
                    uiElement.OnFocusGained();
                }
            }
        }

        internal void FocusLost()
        {
            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnFocusLost != null)
                {
                    uiElement.OnFocusLost();
                }
            }
        }

        internal void MouseMove(Window cWindow, MouseButton button)
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

            foreach (Gadget uiElement in AssetList)
            {
                if (uiElement.OnMouseMove != null) // this one is passed regardless of intersection for things like button highlighting
                {
                    uiElement.OnMouseMove(button);
                }
            }
        }

        internal void KeyPressed(Key key)
        {
            foreach (Gadget uiElement in AssetList)
            {
                // check if the UI element is focused.
                if (uiElement.Focused
                    && uiElement.OnKeyPressed != null)
                {
                    uiElement.OnKeyPressed(key);
                }
            }
        }

        internal void KeyReleased(Key key)
        {
            foreach (Gadget uiElement in AssetList)
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