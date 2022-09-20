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
        public override void AddAsset(Renderer cRenderer, Gadget gadget)
        {
            NCLogging.Log($"Creating new Gadget::{gadget.GetType().Name}");
            Assets.Add(gadget);
        }

        public override void RemoveAsset(Renderer cRenderer, Gadget gadget)
        {
            if (!Assets.Contains(gadget)) _ = new NCException($"Attempted to remove a gadget of type ({gadget.GetType().Name} that is not in the UI Manager - you must add it first!", 135, "Called UIManager::RemoveElement with a gadget property that does not correspond to a Gadget loaded by the UI Manager!", NCExceptionSeverity.Warning);
            NCLogging.Log($"Removing a Gadget::{gadget.GetType().Name} from UIManager");
            Assets.Remove(gadget);
        }

        /// <summary>
        /// Renders all UI elements.
        /// </summary>
        /// <param name="cRenderer">The UI element to render.</param>
        internal override void Update(Renderer cRenderer)
        {
            foreach (Gadget uiElement in Assets)
            {
                if (uiElement.Size == default) _ = new NCException($"Attempted to draw a gadget with no size, you will not see it!", 122, "Gadget::Size = (0,0)!", NCExceptionSeverity.Warning, null, true);
                if (uiElement.OnRender != null)
                {
                    uiElement.OnRender(cRenderer);
                }
            }
        }
    }
}