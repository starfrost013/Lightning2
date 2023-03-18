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
            
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
            Lightning.Renderer.AddRenderable(new TextBlock("Scene1", "Hello from MainScene", "DebugFont", new Vector2(300, 300), Color.Red));

            // change the scene
            if (Lightning.Renderer.EventWaiting)
            {
                SdlRenderer sdlRenderer = (SdlRenderer)Lightning.Renderer;
                // DEPRECATED DO NOT USE!
                if (sdlRenderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN) SetCurrentScene("Scene2");
            }
        }
    }
}
