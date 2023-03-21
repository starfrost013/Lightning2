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
            // background has to be different to environmental light colour lol.
            Lightning.Renderer.Clear(Color.FromArgb(255, 255, 255, 255));

            // set environmental light
            LightManager.SetEnvironmentalLight(Color.FromArgb(255, 0, 0, 0));

            Light newLight = new("Light1")
            {
                Brightness = 255, // range 0-255, (255 - value) = lowest alpha range in environmental light
                Range = 7,
                Position = new Vector2(130, 100),
            };

            Light newColouredLight = new("Light2")
            {
                Brightness = 140,
                Range = 5,
                Position = new Vector2(150, 550),
                LightColor = Color.Green
            };

            Light newColouredLight2 = new("Light3")
            {
                Brightness = 190,
                Range = 6,
                Position = new Vector2(425, 550),
                LightColor = Color.Green
            };

            Light newColouredLight3 = new("Light4")
            {
                Brightness = 255,
                Range = 7,
                Position = new Vector2(750, 550),
                LightColor = Color.Green
            };

            Lightning.Renderer.AddRenderable(newLight);
            Lightning.Renderer.AddRenderable(newColouredLight);
            Lightning.Renderer.AddRenderable(newColouredLight2);
            Lightning.Renderer.AddRenderable(newColouredLight3);

            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Lighting example", "DebugFont", new Vector2(100, 100), Color.Black)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {

        }
    }
}
