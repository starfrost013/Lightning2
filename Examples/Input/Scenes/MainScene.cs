namespace BasicScene
{
    public class MainScene : Scene
    {
        private TextBlock? PressedText { get; set; }

        private TextBlock? ClickedText { get; set; }

        private TextBlock? MousePosText { get; set; }

        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Tree.AddRenderable(new TextBlock("Text1", "Input example", "DebugFont", new Vector2(100, 100), Color.White)); // no fonts loaded so we use the debug font
            PressedText = Lightning.Tree.AddRenderable(new TextBlock("Text2", "Last keypress: ", "DebugFont", new Vector2(100, 120), Color.White)); // no fonts loaded so we use the debug font
            ClickedText = Lightning.Tree.AddRenderable(new TextBlock("Text3", "Last mouse click: ", "DebugFont", new Vector2(100, 140), Color.White)); // no fonts loaded so we use the debug font
            MousePosText = Lightning.Tree.AddRenderable(new TextBlock("Text4", "Last mouse motion: ", "DebugFont", new Vector2(100, 160), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
            Debug.Assert(PressedText != null
                && ClickedText != null
                && MousePosText != null);

            // DEPRECATED DO NOT USE
            SdlRenderer renderer = (SdlRenderer)Lightning.Renderer;

            // DEPRECATED DO NOT USE
            if (renderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN)
            {
                Key key = (Key)renderer.LastEvent.key;
                PressedText.Text = $"Last keypress: {key}, modifiers: {key.Modifiers}, repeated: {key.Repeated}";
            }
            else if (renderer.LastEvent.type == SDL_EventType.SDL_MOUSEBUTTONDOWN)
            {
                MouseButton button = (MouseButton)renderer.LastEvent.button;
                ClickedText.Text = $"Last mouse click: {button.Button}, position: {button.Position}";
            }
            else if (renderer.LastEvent.type == SDL_EventType.SDL_MOUSEMOTION)
            {
                MouseButton button = (MouseButton)renderer.LastEvent.motion;
                MousePosText.Text = $"Last mouse motion: {button.Button}, position: {button.Position}, velocity: {button.Velocity}";
            }
        }
    }
}
