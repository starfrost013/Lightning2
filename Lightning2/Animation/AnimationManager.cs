﻿namespace LightningGL
{
    /// <summary>
    /// AnimationManager
    /// 
    /// implements Lightning 1.1's new animation system
    /// 
    /// September 5, 2022
    /// </summary>
    public class AnimationAssetManager : AssetManager<Animation>
    {
        public override Animation? AddAsset(Animation? asset)
        {
            if (asset == null) return null;

            if (!File.Exists(asset.Path)) NCError.ShowErrorBox("Attempted to load a nonexistent animation file.", 138,
                "Animation::Load called with Path property that does not point to a valid Animation JSON file!", NCErrorSeverity.FatalError);

            NCLogging.Log($"Deserialising animation JSON from {asset.Path}...");

            // try to deserialise
            try
            {
                string tempPath = asset.Path;
                asset = JsonConvert.DeserializeObject<Animation>(File.ReadAllText(asset.Path));

                if (asset == null)
                {
                    NCError.ShowErrorBox($"A fatal error occurred while deserialising an animation JSON.", 140,
                    "Animation::Load - JsonConvert::DeserializeObject returned null", NCErrorSeverity.FatalError);
                    return null;
                }

                // this needs to be set again currently
                asset.Path = tempPath;

                NCLogging.Log($"Validating animation JSON from {asset.Path}...");
                asset.Validate();

                // load the asset
                asset.Loaded = true;
                Lightning.Renderer.AddRenderable(asset);
                return asset;
            }
            catch (Exception err)
            {
                NCError.ShowErrorBox($"A fatal error occurred while deserialising an animation JSON. See base exception information for further information.", 139,
                    "Animation::Load - fatal error in call to JsonConvert::DeserializeObject", NCErrorSeverity.FatalError, err);
                return null; 
            }
        }

        public void Shutdown()
        {

        }
    }
}
