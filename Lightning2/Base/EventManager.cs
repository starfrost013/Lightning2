
namespace LightningGL
{
    /// <summary>
    /// EventManager
    /// 
    /// Manages events and passes them to renderables.
    /// Not technically an asset manager, due to the fact that events are just delegate types which don't have a common root class (therefore no class for the asset manager
    /// to actually use).
    /// Additionally not an API for public use, but an internal engine API.
    /// </summary>
    internal static class EventManager
    {
        internal static void FireMousePressed(MouseButton button, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, button.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if ((intersects
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnMousePressed?.Invoke(button);
                }

                if (renderable.Children.Count > 0) FireMousePressed(button, renderable);
            }
        }

        internal static void FireMouseReleased(MouseButton button, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, button.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if ((intersects
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnMouseReleased?.Invoke(button);
                }

                // children
                if (renderable.Children.Count > 0) FireMouseReleased(button, renderable);
            }
        }

        internal static void FireMouseEnter(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseEnter?.Invoke();
                if (renderable.Children.Count > 0) FireMouseEnter(renderable);
            }
        }

        internal static void FireMouseLeave(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseLeave?.Invoke();
                if (renderable.Children.Count > 0) FireMouseLeave(renderable);
            }
        }

        internal static void FireFocusGained(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnFocusGained?.Invoke();
                if (renderable.Children.Count > 0) FireFocusGained(renderable);
            }
        }

        internal static void FireFocusLost(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnFocusLost?.Invoke();
                if (renderable.Children.Count > 0) FireFocusLost(renderable);
            }
        }

        internal static void FireMouseMove(MouseButton button, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseMove?.Invoke(button);
                if (renderable.Children.Count > 0) FireMouseMove(button, renderable);
            }
        }

        internal static void FireKeyPressed(Key key, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyPressed?.Invoke(key);
                    if (renderable.Children.Count > 0) FireKeyPressed(key, renderable);
                }
            }
        }

        internal static void FireKeyReleased(Key key, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyPressed?.Invoke(key);
                    if (renderable.Children.Count > 0) FireKeyReleased(key, renderable);
                }
            }
        }
    }
}
