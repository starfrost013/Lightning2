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
            Lightning.Tree.AddRenderable(new Texture("ImageTexture", 1920, 1080, false, @"Content\Logo.png"));
            Lightning.Tree.AddRenderable(new TextBlock("Text1", "Hello from MainScene - press any key to switch scene", "DebugFont", new Vector2(300, 300), Color.Red));
        }

        public override void SwitchFrom(Scene newScene)
        {
            Lightning.Tree.RemoveRenderableByName("Text1");
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
