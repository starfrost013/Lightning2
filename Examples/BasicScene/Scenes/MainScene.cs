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

        public override void SwitchTo(Scene oldScene)
        {
            
        }

        public override void SwitchAway(Scene newScene)
        {
            
        }

        public override void Render()
        {
            PrimitiveRenderer.DrawText("Hello from MainScene", new Vector2(300, 300), Color.Red);

            // change the scene
            if (Lightning.Renderer.EventWaiting)
            {
                if (renderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN) SceneManager.SetCurrentScene("Scene2");
            }
        }
    }
}
