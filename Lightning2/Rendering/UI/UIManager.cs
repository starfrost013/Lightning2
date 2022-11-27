namespace LightningGL
{
    /// <summary>
    /// UIManager
    /// 
    /// A simple UI manager for Lightning.
    /// </summary>
    public class UIAssetManager : AssetManager<Gadget>
    {
        /// <summary>
        /// Adds a <see cref="Gadget"/> to the UI manager.
        /// </summary>
        /// <param name="gadget">The <see cref=""/></param>
        public override Gadget AddAsset(Gadget gadget)
        {
            Lightning.Renderer.AddRenderable(gadget);
            return gadget;
        }

        public override void RemoveAsset(Gadget gadget)
        {
            if (!Lightning.Renderer.ContainsRenderable(gadget.Name))
            {
                NCError.ShowErrorBox($"Attempted to remove a gadget of type ({gadget.GetType().Name} that is not in the UI Manager - you must add it first!", 135, "Called UIManager::RemoveElement with a gadget property that does not correspond to a Gadget loaded by the UI Manager!", NCErrorSeverity.Warning);
                return;
            }

            NCLogging.Log($"Removing a Gadget::{gadget.GetType().Name} from UIManager ({gadget.Name}");
            Lightning.Renderer.RemoveRenderable(gadget);
        }
    }
}