
namespace BasicScene
{
    public class MainScene : Scene
    {
        private Texture? Texture { get; set; }

        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            // create a texture
            Texture = new("Texture1", 512, 512);
            Lightning.Renderer.AddRenderable(Texture);

            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Texture API example", "DebugFont", new(0, 550), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
            Debug.Assert(Texture != null);

            int rX = Random.Shared.Next(0, (int)Texture.Size.X); // exclusive upper bound
            int rY = Random.Shared.Next(0, (int)Texture.Size.Y);

            // you shouldn't lock/unlock constantly, it's not very efficient
            // this is solely done for the purpose of this demo ONLY
            Texture.SetPixel(rX, rY, Color.White);
        }
    }
}
