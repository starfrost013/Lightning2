using System.Numerics;

namespace Lightning2
{
    public class Particle
    {
        internal Texture Texture { get; private set; }

        public Vector2 Velocity { get; set; }
        public Particle(Texture texture)
        {
            Texture = texture;
        }

        public void Load(Window cWindow)
        {
            Texture.Load(cWindow);
        }

        

    }
}
