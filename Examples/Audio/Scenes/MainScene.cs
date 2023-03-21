
namespace BasicScene
{
    public class MainScene : Scene
    {
        private Audio DeepBlue { get; set; }

        private Audio Royksopp { get; set; }

        public override void Start()
        {
            // supported formats: flac, ogg, mp3, midi (mods are broken)
            // positional audio and volume controls are also supported
            // get the audio files
            DeepBlue = Lightning.Renderer.AddRenderable(new Audio("deepblue", @"Content\deepbluecalm.flac"));
            Royksopp = Lightning.Renderer.AddRenderable(new Audio("royksopp", @"Content\royksopp.mp3"));
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Audio example!", "DebugFont", new Vector2(100, 100), Color.White)); // no fonts loaded so we use the debug font
            Lightning.Renderer.AddRenderable(new TextBlock("Text2", "Left mouse to play audio 1", "DebugFont", new Vector2(100, 120), Color.White)); // no fonts loaded so we use the debug font
            Lightning.Renderer.AddRenderable(new TextBlock("Text3", "Right mouse to play audio 2", "DebugFont", new Vector2(100, 160), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
            // DEPRECATED DO NOT USE
            SdlRenderer renderer = (SdlRenderer)Lightning.Renderer;

            if (renderer.EventWaiting)
            {
                switch (renderer.LastEvent.type)
                {
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        MouseButton button = (MouseButton)renderer.LastEvent.button;

                        switch (button.Button)
                        {
                            case SDL_MouseButton.Left:
                                DeepBlue.Play();
                                Royksopp.Stop();
                                break;
                            case SDL_MouseButton.Right:
                                DeepBlue.Stop();
                                Royksopp.Play();
                                break;
                        }

                        break;
                }
            }
        }
    }
}
