namespace LightningGL
{
    /// <summary>
    /// FontManager
    /// 
    /// Provides functions for managing fonts and drawing text.
    /// </summary>
    public class TextAssetManager : AssetManager<Text>
    {
        public override Text? AddAsset(Text asset)
        {
            if (asset != null)
            {
                try
                {
                    Lightning.Renderer.AddRenderable(asset);
                }
                catch (Exception) // NC Exception
                {
                    return null;
                }
            }
            else
            {
                NCError.ShowErrorBox("Passed null font to FontAssetManager::AddAsset!", 184, "FontAssetManager::AddAsset asset parameter was null!", NCErrorSeverity.FatalError);
            }

            return asset;
        }
    }
}