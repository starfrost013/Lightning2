﻿using LightningGL;
using static NuCore.SDL2.SDL;
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
            PrimitiveRenderer.DrawText(cWindow, "Hello from MainScene", new Vector2(300, 300), Color.Red);

            // change the scene
            if (cWindow.EventWaiting)
            {
                if (cWindow.LastEvent.type == SDL_EventType.SDL_KEYDOWN) SceneManager.SetCurrentScene("Scene2");
            }
        }
    }
}
