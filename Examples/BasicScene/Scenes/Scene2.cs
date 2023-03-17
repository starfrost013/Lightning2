using static LightningBase.SDL; // not required for project template
using LightningGL;
using static LightningGL.Lightning; // not required for project template
using System.Drawing;
using System.Numerics;
using LightningBase;

namespace BasicScene
{
    public class Scene2 : Scene
    {
        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Hello from Scene2", "DebugFont", new(300, 300), Color.PaleTurquoise));
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {


            // change the scene
            if (Lightning.Renderer.EventWaiting)
            {
                if (renderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN) Lightning.SetCurrentScene("MainScene");
            }
        }
    }
}
