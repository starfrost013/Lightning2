// see globalusings.cs for namespaces used here

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

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Tree.AddRenderable(new TextBlock("Text1", "Click to add a Local Setting! All settings will be saved to LocalSettings.ini when you exit", "DebugFont",
                new(300, 300), Color.Red));
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {
            // DEPRECATED DO NOT USE
            SdlRenderer sdlRenderer = (SdlRenderer)Lightning.Renderer;

            if (sdlRenderer.EventWaiting)
            {
                switch (sdlRenderer.LastEvent.type)
                {
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        string value1 = Random.Shared.Next(0, 1000000).ToString();
                        string value2 = Random.Shared.Next(0, 1000000).ToString();
                        LocalSettings.AddValue($"Demonstration", $"test{value1}", $"test{value2}");
                        Logger.Log($"Added setting to Demonstration section, key {value1}, value {value2}!");
                        break;
                }
            }


        }
    }
}
