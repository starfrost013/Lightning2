using static LightningBase.SDL; // not required for project template
using LightningGL;
using static LightningGL.Lightning; // not required for project template
using System.Drawing;
using System.Numerics;

namespace BasicScene
{
    public class MainScene : Scene
    {
        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {

            // Load a texture from a file using Texture::Load
            Texture texture = new("Texture1", 18, 11) // yes thats the image size blame my lazy cropping
            {
                Path = "Content/ParticleEffect.png",
            };

            Lightning.Tree.AddRenderable(texture);

            // Create a particle effect.
            ParticleEffect particleEffect = new("ParticleEffect1", texture)
            {
                MaxNumberCreatedEachFrame = 3,
                FrameSkipBetweenCreatingParticles = 2,
                Amount = 200,
                Lifetime = 5000,
                Variance = 25,
                Position = new(100, 300),
                Velocity = new(0.1f, 0.1f), // it's a float so use f
                Mode = ParticleMode.Explode,
            };

            Lightning.Tree.AddRenderable(particleEffect);

            Lightning.Tree.AddRenderable(new TextBlock("Text1", "Particle effect example", "DebugFont", new(100, 100), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {

        }
    }
}
