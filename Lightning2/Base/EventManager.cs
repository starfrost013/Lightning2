
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
        internal static void FireOnMousePressed(MouseButton mouseButton, Renderable? parent = null, bool isRootOfCurrentCall = true)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null
                && !isRootOfCurrentCall)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                mouseButton.Position = new Vector2
                    (cameraPosition.X + mouseButton.Position.X,
                    cameraPosition.Y + mouseButton.Position.Y); 
            }

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(mouseButton.ToString());

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, mouseButton.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if ((intersects
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnMouseDown?.Invoke(binding, mouseButton);
                }

                if (renderable.Children.Count > 0) FireOnMousePressed(mouseButton, renderable, false);
            }
        }

        internal static void FireOnMouseReleased(MouseButton mouseButton, Renderable? parent = null, bool isRootOfCurrentCall = true)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null
                && isRootOfCurrentCall)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                mouseButton.Position = new Vector2
                    (cameraPosition.X + mouseButton.Position.X,
                    cameraPosition.Y + mouseButton.Position.Y);
            }

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(mouseButton.ToString());

            foreach (Renderable renderable in renderables)
            {
                bool intersects = AABB.Intersects(renderable, mouseButton.Position);

                // check if it is focused...
                renderable.Focused = intersects;

                if (intersects
                    || renderable.CanReceiveEventsWhileUnfocused)
                {
                    renderable.OnMouseUp?.Invoke(binding, mouseButton);
                }

                // children
                if (renderable.Children.Count > 0) FireOnMouseReleased(mouseButton, renderable, false);
            }
        }

        internal static void FireOnMouseEnter(Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseEnter?.Invoke();
                if (renderable.Children.Count > 0) FireOnMouseEnter(renderable);
            }
        }

        internal static void FireOnMouseLeave(Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseLeave?.Invoke();
                if (renderable.Children.Count > 0) FireOnMouseLeave(renderable);
            }
        }

        internal static void FireOnMouseWheel(MouseButton button, Renderable? parent = null, bool isRootOfCurrentCall = true)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null
                && isRootOfCurrentCall)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseWheel?.Invoke(new("MOUSEWHEEL", ""), button);
                if (renderable.Children.Count > 0) FireOnMouseWheel(button, renderable, false);
            }
        }

        internal static void FireOnMouseMove(MouseButton button, Renderable? parent = null, bool isRootOfCurrentCall = true)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = Lightning.Renderer.Settings.Camera;

            Vector2 cameraPosition = currentCamera.Position;

            if (currentCamera != null
                && isRootOfCurrentCall)
            {
                // get the real position that we are checking
                // here we have to ADD the current position to the camera position (top left)
                button.Position = new Vector2
                    (cameraPosition.X + button.Position.X,
                    cameraPosition.Y + button.Position.Y);
            }

            foreach (Renderable renderable in renderables)
            {
                renderable.OnMouseMove?.Invoke(new("MOUSEMOVE", ""), button);
                if (renderable.Children.Count > 0) FireOnMouseMove(button, renderable, false);
            }
        }

        internal static void FireOnFocusGained(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnFocusGained?.Invoke();
                if (renderable.Children.Count > 0) FireOnFocusGained(renderable);
            }
        }

        internal static void FireOnFocusLost(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnFocusLost?.Invoke();
                if (renderable.Children.Count > 0) FireOnFocusLost(renderable);
            }
        }

        internal static void FireOnKeyDown(Key key, Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(key.ToString());

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyDown?.Invoke(binding, key);
                    if (renderable.Children.Count > 0) FireOnKeyDown(key, renderable);
                }
            }
        }

        internal static void FireOnKeyUp(Key key, Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(key.ToString());

            foreach (Renderable renderable in renderables)
            {
                // check if the UI element is focused.
                if ((renderable.Focused
                    || renderable.CanReceiveEventsWhileUnfocused))
                {
                    renderable.OnKeyUp?.Invoke(binding, key);
                    if (renderable.Children.Count > 0) FireOnKeyUp(key, renderable);
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
                renderable.OnAnimationStart?.Invoke();
                if (renderable.Children.Count > 0) FireOnAnimationStart(renderable);
            }
        }

        internal static void FireOnAnimationStop(Renderable? parent = null)
        {
            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            foreach (Renderable renderable in renderables)
            {
                renderable.OnAnimationStop?.Invoke();
                if (renderable.Children.Count > 0) FireOnAnimationStop(renderable);
            }
        }

        internal static void FireOnControllerButtonDown(ControllerButton button, Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(button.Features.ToString());

            foreach (Renderable renderable in renderables)
            {
                renderable.OnControllerButtonDown?.Invoke(binding, button);
                if (renderable.Children.Count > 0) FireOnControllerButtonDown(button, parent);
            }
        }

        internal static void FireOnControllerButtonUp(ControllerButton button, Renderable? parent = null)
        {
            Debug.Assert(InputMethodManager.CurrentMethod != null);

            // render all children 
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            InputBinding? binding = InputMethodManager.CurrentMethod.GetBindingByBind(button.Features.ToString());

            foreach (Renderable renderable in renderables)
            {
                renderable.OnControllerButtonUp?.Invoke(binding, button);
                if (renderable.Children.Count > 0) FireOnControllerButtonUp(button, parent);
            }
        }
    }
}
