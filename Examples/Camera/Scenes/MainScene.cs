
using LightningGL;

namespace BasicScene
{
    public class MainScene : Scene
    {
        private Camera Camera { get; set; } // not super recommended ??

        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            // setup camera
            Camera = new(CameraType.Follow);
            Lightning.Renderer.SetCurrentCamera(Camera);

            Lightning.Renderer.AddRenderable(new Pixel("Pixel1", new Vector2(20, 100), Color.FromArgb(255, 100, 100, 255))); // draw a pixel @ 0,0, colour ARGB 255,100,100,255
            Lightning.Renderer.AddRenderable(new Line("Line1", new Vector2(40, 100), new Vector2(40, 250), Color.Blue)); // draw a line from 10,0 to 50,0, thickness 5, colour blue
            Lightning.Renderer.AddRenderable(new Ellipse("Ellipse1", new Vector2(40, 100), new Vector2(25, 25), Color.Yellow, false)); // draw an anti-aliased, yellow unfilled circle size (25,25) at (40,100)
            Lightning.Renderer.AddRenderable(new Ellipse("Ellipse2", new Vector2(93, 100), new Vector2(25, 25), Color.Yellow, true)); // draw a yellow filled circle size (25,25) at (93,100)
            Lightning.Renderer.AddRenderable(new Rectangle("Rectangle1", new Vector2(120, 100), new Vector2(25, 25), Color.Orange, false)); // draw an orange unfilled rectangle size (25,25) at (120,100)
            Lightning.Renderer.AddRenderable(new Rectangle("Rectangle2", new Vector2(150, 100), new Vector2(25, 25), Color.OrangeRed, true)); // draw an orange-red filled rectangle size (25,25) at (150,100)
            Lightning.Renderer.AddRenderable(new TextBlock("TextBlock1", "Camera example", "DebugFont", new Vector2(300, 300), Color.White)); // no fonts loaded so we use the debug font

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
                    // DEPRECATED DO NOT USE
                    // THIS IS A TERRIBLE IMPLEMENTATION DO NOT USE
                    case SDL_EventType.SDL_KEYDOWN: // Key is held down.
                        Key key = (Key)sdlRenderer.LastEvent.key;

                        string keyString = key.ToString();

                        switch (keyString)
                        {
                            case "LEFT":
                            case "A":
                                Camera.Position -= new Vector2(10, 0);
                                break;
                            case "RIGHT":
                            case "D":
                                Camera.Position += new Vector2(10, 0);
                                break;
                            case "UP":
                            case "W":
                                Camera.Position -= new Vector2(0, 10);
                                break;
                            case "DOWN":
                            case "S":
                                Camera.Position += new Vector2(0, 10);
                                break;
                        }
                        break;
                }
            }
        }
    }
}
