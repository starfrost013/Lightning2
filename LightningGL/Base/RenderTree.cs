
namespace LightningGL
{
    public class RenderTree
    {
        public List<Renderable> Tree = new();

        /// <summary>
        /// Adds a renderable to the renderer hierarchy.
        /// </summary>
        public virtual T AddRenderable<T>(T renderable, Renderable? parent = null) where T : Renderable
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            Logger.Log($"Adding renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");

            if (parent == null)
            {
                Lightning.Renderer.Renderables.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }
            else
            {
                // check that it contains the renderable
                if (!ContainsRenderable(parent.Name)) Logger.LogError($"Tried to add a renderable with a parent " +
                    $"that is not in the object hierarchy!", 194, LoggerSeverity.FatalError);

                renderable.Parent = parent;

                parent.Children.Add(renderable);

                // guaranteed never null
                renderable.OnCreate();
            }

            return renderable;
        }

        /// <summary>
        /// Removes a renderable.
        /// </summary>
        public virtual void RemoveRenderable(Renderable renderable, Renderable? parent = null)
        {
            string parentName = (parent == null) ? "Root" : parent.Name;

            Logger.Log($"Removing renderable of type {renderable.GetType().Name} ({renderable.Name}) - parent {parentName}");


            for (int childId = 0; childId < renderable.Children.Count; childId++)
            {
                Renderable child = renderable.Children[childId];
                child.StopCurrentAnimation();
                child.OnDestroy();

                if (child.Children.Count > 0) RemoveRenderable(child);

                renderable.Children.Remove(child);
                childId--;
            }

            renderable.StopCurrentAnimation();
            renderable.OnDestroy();

            // if there's no parent...
            // how do we take into account the case where it's not actually in its parent?
            if (parent == null)
            {
                Lightning.Renderer.Renderables.Remove(renderable);
            }
            else
            {
                parent.Children.Remove(renderable);
            }

        }

        public virtual void RemoveAllChildren(Renderable renderable)
        {
            Logger.Log($"Removing all children of renderable of type {renderable.GetType().Name} ({renderable.Name})");

            for (int renderableId = 0; renderableId < renderable.Children.Count; renderableId++)
            {
                Renderable child = renderable.Children[renderableId];
                RemoveRenderable(child, renderable);
            }
        }

        public virtual Renderable? GetRenderableByName(string name, Renderable? parent = null)
        {
            // iterate through either the root or the child list depending on if the parent paremter was provided
            List<Renderable> renderables = (parent == null) ? Lightning.Renderer.Renderables : parent.Children;

            Renderable? foundRenderable = null;

            foreach (Renderable renderable in renderables)
            {
                // kind of a stupid hack but it's better than using break in a foreach lol
                if (renderable.Children.Count > 0)
                {
                    Renderable? newRenderable = GetRenderableByName(name, renderable);
                    if (newRenderable != null) foundRenderable = newRenderable;
                }

                if (renderable.Name == name) foundRenderable = renderable;
            }

            return foundRenderable;
        }

        public virtual void RemoveRenderableByName(string name)
        {
            Renderable? renderable = GetRenderableByName(name);

            if (renderable == null)
            {
                Logger.LogError($"Tried to remove nonexistent renderable name {name}", 190, LoggerSeverity.FatalError);
                return;
            }

            RemoveRenderable(renderable);
        }

        public virtual bool ContainsRenderable(string name) => GetRenderableByName(name) != null;

        internal int CountRenderables(Renderable? parent = null, int initialCount = 0)
        {
            List<Renderable> renderables = (parent == null) ? Tree : parent.Children;

            int count = initialCount;

            foreach (Renderable renderable in renderables)
            {
                count++;
                if (renderable.Children.Count > 0) count = CountRenderables(renderable, count);
            }

            return count;
        }

    }
}
