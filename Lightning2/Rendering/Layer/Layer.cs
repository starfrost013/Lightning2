using System.Collections.Generic;
using System.Numerics;

namespace LightningGL
{
    /// <summary>
    /// Layer
    /// 
    /// June 16, 2022
    /// 
    /// Defines a rendering layer. A rendering layer is a layer of rendering that contains any object and also controls the rendering of any object.
    /// Rendering commands are sent to the layer which then processes them.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Optional name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If true, screen-space coordinates will be used for this layer instead of world-space coordinates.
        /// </summary>
        public bool SnapToScreen { get; set; }

        /// <summary>
        /// The list of <see cref="Renderables"/> in this layer.
        /// </summary>
        public List<Renderable> Renderables { get; private set; }

        public Layer(string nName)
        {
            Name = nName;
            Renderables = new List<Renderable>();   
        }

        public void AddRenderable(Renderable renderable) => Renderables.Add(renderable);

        public void Render(Window cWindow)
        {
            Camera curCamera = cWindow.Settings.Camera;

            foreach (Renderable renderable in Renderables)
            {
                if (curCamera != null
                    && !SnapToScreen)
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
