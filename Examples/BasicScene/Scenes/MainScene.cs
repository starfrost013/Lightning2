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
            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Hello from MainScene", "DebugFont", new Vector2(300, 300), Color.Red));
        }

        public override void SwitchFrom(Scene newScene)
        {
            Lightning.Renderer.RemoveRenderableByName("Text1");
        }

        public override void Render()
        {

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
