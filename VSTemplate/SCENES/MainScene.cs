using LightningGL;
using static NuCore.SDL2.SDL;
using System.Drawing;
using System.Numerics;

namespace $safeprojectname$
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

        public override void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawText(cWindow, "Hello World!", new Vector2(300, 300), Color.Red);
        }
    }
}
