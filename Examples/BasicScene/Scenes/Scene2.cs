namespace BasicScene
{
    public class Scene2 : Scene
    {
        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Tree.AddRenderable(new TextBlock("Text2", "Hello from Scene2", "DebugFont", new(300, 300), Color.PaleTurquoise));
        }

        public override void SwitchFrom(Scene newScene)
        {
            Lightning.Tree.RemoveRenderableByName("Text2");
        }

        public override void Render()
        {
            // change the scene
            if (Lightning.Renderer.EventWaiting)
            {
                SdlRenderer sdlRenderer = (SdlRenderer)Lightning.Renderer;
                // DEPRECATED DO NOT USE!
                if (sdlRenderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN) SetCurrentScene("MainScene");
            }
        }
    }
}
