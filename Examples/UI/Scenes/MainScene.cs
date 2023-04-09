using LightningUtil;

namespace BasicScene
{
    public class MainScene : Scene
    {
        private Button? Button1 { get; set; }

        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Button1 = new Button("Button1", "DebugFont") // 150 char capacirty
            {
                Size = new(100, 30),
                BackgroundColor = Color.White,
                PressedColor = Color.Gray,
                HoverColor = Color.LightGray,
                BorderColor = Color.Yellow,
                BorderSize = new(3, 3),
                ForegroundColor = Color.Black,
                Position = new(100, 300),
                Text = "Button 1",
                Filled = true
            };

            // register event handlers
            Button1.OnMouseDown += OnMouseDown;
            Button1.OnMouseUp += OnMouseUp;

            Lightning.Tree.AddRenderable(Button1);

            Lightning.Tree.AddRenderable(new TextBlock("Text1", "UI example", "DebugFont", new(100, 100), Color.White)); // no fonts loaded so we use the debug font

        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {

        }

        void OnMouseDown(InputBinding? binding, MouseButton button)
        {
            Debug.Assert(Button1 != null);
            // call base handler
            Button1.MousePressed(binding, button);
            Logger.Log("Button held down");
        }

        void OnMouseUp(InputBinding? binding, MouseButton button)
        {
            Debug.Assert(Button1 != null);
            // call base handler
            Button1.MouseReleased(binding, button);
            Logger.Log("Button no longer held down");

        }
    }
}
