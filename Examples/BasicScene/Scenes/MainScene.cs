using LightningGL;
using System;
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

        public override void Render(Window cWindow)
        {
            PrimitiveRenderer.DrawText(cWindow, "Scene - MainScene", new Vector2(300, 300), Color.AliceBlue);
        }
    }
}
