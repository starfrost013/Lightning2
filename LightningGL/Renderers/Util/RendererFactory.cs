
namespace LightningGL
{
    /// <summary>
    /// RendererFactory
    /// 
    /// Factory class for renderers
    /// </summary>
    internal static class RendererFactory
    {
        internal static Renderer GetRenderer(Renderers renderer)
        {
            switch (renderer)
            { 
                case Renderers.Sdl: return new SdlRenderer();
            }

            // default to SDL if we use an invalid renderer
            Logger.Log($"Tried to specify invalid renderer, falling back to SDL2!");
            return new SdlRenderer();
        }
    }
}
