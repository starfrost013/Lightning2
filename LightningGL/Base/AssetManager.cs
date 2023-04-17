﻿namespace LightningGL
{
    /// <summary>
    /// Base class for Lightning asset managers.
    /// </summary>
    public abstract class AssetManager<T> where T : Renderable
    {
        public virtual List<T> GetAssets()
        {
            Debug.Assert(Lightning.Renderer != null);

            List<T> assets = new();

            foreach (Renderable renderable in Lightning.Renderer.Renderables)
            {
                if (renderable is T)
                {
                    assets.Add((T)renderable);
                }
            }

            return assets;
        }

        public virtual int CountAssets()
        {
            Debug.Assert(Lightning.Renderer != null);

            int count = 0;

            // considered doing getassets.count here but building a list would be slower
            foreach (Renderable renderable in Lightning.Renderer.Renderables)
            {
                if (renderable is T)
                {
                    count++;
                }
            }

            return count;
        }

        public virtual void RemoveAllAssets()
        {
            Debug.Assert(Lightning.Renderer != null);

            foreach (Renderable renderable in Lightning.Renderer.Renderables)
            {
                Lightning.Tree.RemoveRenderable(renderable);
            }
        }

        internal virtual void Update()
        {

        }
    }
}
