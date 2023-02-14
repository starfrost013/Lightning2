
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
        internal static void FireMousePressed(MouseButton mouseButton, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                mouseButton.Position = new Vector2
                    (cameraPosition.X + mouseButton.Position.X,
                    cameraPosition.Y + mouseButton.Position.Y); 
            }

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, mouseButton.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if ((intersects
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnMousePressed?.Invoke(mouseButton);
                }

                if (renderable.Children.Count > 0) FireMousePressed(mouseButton, renderable);
            }
        }

        internal static void FireMouseReleased(MouseButton mouseButton, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                mouseButton.Position = new Vector2
                    (cameraPosition.X + mouseButton.Position.X,
                    cameraPosition.Y + mouseButton.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, mouseButton.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if ((intersects
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnMouseReleased?.Invoke(mouseButton);
                }

                // children
                if (renderable.Children.Count > 0) FireMouseReleased(mouseButton, renderable);
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

        internal static void FireMouseWheel(MouseButton button, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseWheel?.Invoke(button);
                if (renderable.Children.Count > 0) FireMouseWheel(button, renderable);
            }
        }

        internal static void FireMouseMove(MouseButton mouseButton, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                mouseButton.Position = new Vector2
                    (cameraPosition.X + mouseButton.Position.X,
                    cameraPosition.Y + mouseButton.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseMove?.Invoke(mouseButton);
                if (renderable.Children.Count > 0) FireMouseMove(mouseButton, renderable);
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
                    renderable.OnKeyReleased?.Invoke(key);
                    if (renderable.Children.Count > 0) FireKeyReleased(key, renderable);
                }
            }
        }

        internal static void FireOnSwitchFromScene(Scene oldScene, Scene newScene, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnSwitchFromScene?.Invoke(oldScene, newScene);
                    if (renderable.Children.Count > 0) FireOnSwitchFromScene(oldScene, newScene, renderable);
                }
            }
        }

        internal static void FireOnSwitchToScene(Scene? oldScene, Scene newScene, Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnSwitchToScene?.Invoke(oldScene, newScene);
                    if (renderable.Children.Count > 0) FireOnSwitchToScene(oldScene, newScene, renderable);
                }
            }
        }

        internal static void FireOnAnimationStart(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if (renderable.Focused)
                {
                    renderable.OnAnimationStart?.Invoke();
                    if (renderable.Children.Count > 0) FireOnAnimationStart(renderable);
                }
            }
        }

        internal static void FireOnAnimationEnd(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if (renderable.Focused)
                {
                    renderable.OnAnimationStart?.Invoke();
                    if (renderable.Children.Count > 0) FireOnAnimationEnd(renderable);
                }
            }
        }

        internal static void HandleEvents(EventType eventType)
        {
            // input-specific events
            InputMethodManager.CurrentMethod?.HandleEvents(eventType);

            // non-input specific events
            switch (eventType)
            {

            }
        }
    }
}
