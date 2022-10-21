﻿// see globalusings.cs for namespaces used here

namespace Settings
{
    /// <summary>
    /// MainScene
    /// 
    /// The main scene of your Lightning game. 
    /// Add additional scenes by creating classes that inherit from Scene.
    /// </summary>
    public class MainScene : Scene
    {
        public override void Start()
        {
            LocalSettings.AddSection("Demonstration");
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

        public override void Render(Renderer cWindow)
        {
            if (cWindow.EventWaiting)
            {
                switch (cWindow.LastEvent.type)
                {
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        string nValue = Random.Shared.Next(0, 1000000).ToString();
                        LocalSettings.AddValue($"LocalSetting{nValue}", $"test{nValue}", $"test{nValue}");
                        break;
                }
            }

            PrimitiveRenderer.DrawText(cWindow, "Click to add Local Setting! All settings will be saved when you exit", new Vector2(300, 300), Color.Red);
        }
    }
}
