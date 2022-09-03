using System.Collections.Generic;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// TextureManager
    /// 
    /// July 2, 2022
    /// 
    /// Manager for texture rendering.
    /// </summary>
    public static class TextureManager
    {
        /// <summary>
        /// If true, screen-space coordinates will be used for this layer instead of world-space coordinates.
        /// </summary>
        public static bool SnapToScreen { get; set; }

        /// <summary>
        /// The list of <see cref="Textures"/> in this layer.
        /// 
        /// Todo: refactor AnimatedTexture and then change this to a list of textures
        /// </summary>
        public static List<Renderable> Textures { get; private set; }

        static TextureManager()
        {
            Textures = new List<Renderable>();   
        }

        /// <summary>
        /// Adds a texture to the Texture Manager.
        /// </summary>
        /// <param name="renderable">A <see cref="Texture"/> to add to the texture manager</param>
        public static void AddElement(Renderable renderable) => Textures.Add(renderable);

        /// <summary>
        /// Renders all of the textures in the texture manager.
        /// </summary>
        /// <param name="cWindow"></param>
        internal static void Render(Window cWindow)
        {
            Camera curCamera = cWindow.Settings.Camera;

            foreach (Renderable renderable in Textures)
            {
                if (curCamera != null
                    && !renderable.SnapToScreen)
                {
                    renderable.RenderPosition = new Vector2
                        (
                            renderable.Position.X - curCamera.Position.X,
                            renderable.Position.Y - curCamera.Position.Y
                        );
                }
                else
                {
                    renderable.RenderPosition = renderable.Position;
                }

                
                renderable.Draw(cWindow);
            }
        }

    }
}
