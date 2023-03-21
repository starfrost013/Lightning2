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
            AnimatedTexture texture = new("AnimatedTexture1", 256, 256,
                new(0, 3, 1000));

            // add frames to the texture
            texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
            texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
            texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
            texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");
            texture.Position = new(200, 200);
            Lightning.Renderer.AddRenderable(texture);

            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Animated texture example", "DebugFont", new(100, 100), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
        }
    }
}
