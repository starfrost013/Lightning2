namespace LightningGL
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
        public override void AddAsset(Renderer cRenderer, Animation asset)
        {
            if (!File.Exists(asset.Path)) _ = new NCException("Attempted to load a nonexistent animation", 138,
                "Animation::Load called with Path property that does not point to a valid path!", NCExceptionSeverity.FatalError);

            NCLogging.Log($"Deserialising animation JSON from {asset.Path}...");

            // try to deserialise
            try
            {
                string tempPath = asset.Path;
                asset = JsonConvert.DeserializeObject<Animation>(File.ReadAllText(asset.Path));
                // this needs to be set again currently
                asset.Path = tempPath;
                if (asset == null) _ = new NCException($"A fatal error occurred while deserialising an animation JSON.", 140,
                    "Animation::Load - JsonConvert::DeserializeObject returned null", NCExceptionSeverity.FatalError);
            }
            catch (Exception err)
            {
                _ = new NCException($"A fatal error occurred while deserialising an animation JSON. See base exception information for further information.", 139,
                    "Animation::Load - fatal error in call to JsonConvert::DeserializeObject", NCExceptionSeverity.FatalError, err);
            }
            NCLogging.Log($"Validating animation JSON from {asset.Path}...");
            asset.Validate();

            // load the asset
            asset.Loaded = true;
            Assets.Add(asset);
        }

        public Animation GetAnimationWithPath(string path)
        {
            foreach (Animation animation in Assets)
            {
                if (animation.Path == path) return animation;
            }

            return null;
        }

        public Animation GetAnimationWithName(string name)
        {
            foreach (Animation animation in Assets)
            {
                if (animation.Name == name) return animation;
            }

            return null;
        }
    }
}
