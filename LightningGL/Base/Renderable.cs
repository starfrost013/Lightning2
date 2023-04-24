namespace LightningGL
{
    /// <summary>
    /// Renderable
    ///  
    /// Defines a renderable object.
    /// A renderable is an object in Lightning that does
    /// </summary>
    public class Renderable : LightningObject
    {
        /// <summary>
        /// Backing field for <see cref="Name"/>.
        /// </summary>
        private string _name;

        /// <summary>
        /// The Name of this Renderable.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _name = "Unnamed Renderable";
                }
                else
                {
                    _name = value;
                }
            }
        }

        public Renderable(string name) : base()
        {
            Name = name;
            _name = Name; //weird .NET thing (name always sets _name)
        }

        /// <summary>
        /// Draws components of this renderable.
        /// This is always called by the renderer.
        /// </summary>
        internal void DrawComponents()
        {
            foreach (var component in Components)
            {
                if (component.IsOnScreen)
                {
                    component.OnDraw();
                }
            }
        }

        /// <summary>
        /// Draws components of this renderable.
        /// NOT RUN if the renderable is not rendered.
        /// </summary>
        internal void UpdateComponents()
        {
            foreach (var component in Components)
            {
                component.Update();
            }
        }

        public virtual Renderable? GetParent() => Parent;

        public virtual List<Renderable> GetChildren() => Children;

        public virtual Renderable? GetFirstChild()
        {
            if (Children.Count == 0)
            {
                Logger.LogError($"Tried to call Renderable::GetFirstChild on a renderable with no children!", 301, LoggerSeverity.Warning, null, true);
                return null;
            }

            return Children[0];
        }

        public virtual Renderable? GetLastChild()
        {
            if (Children.Count == 0)
            {
                Logger.LogError($"Tried to call Renderable::GetLastChild on a renderable with no children!", 192, LoggerSeverity.Warning, null, true);
                return null;
            }

            return Children[^1];
        }

        public T AddComponent<T>(T component) where T : Component
        {
            Components.Add(component);

            component.OnCreate();
            component.Parent = this;
            return component;
        }

        public T? GetComponent<T>() where T : Component
        {
            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component; 
                }
            }

            Logger.Log($"Tried to get invalid component {typeof(T).Name} of Renderable {Name}!");
            return null;
        }

        internal void DestroyComponents()
        {
            for (int componentId = 0; componentId < Components.Count; componentId++)    
            {
                Component component = Components[componentId];
                component.OnDestroy();

                Components.Remove(component);
                componentId--;
            }
        }
    }
}
