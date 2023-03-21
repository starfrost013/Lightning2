namespace BasicScene
{
    public class MainScene : Scene
    {
        private TextureAtlas? Texture { get; set; }

        public override void Start()
        {
            
        }

        public override void Shutdown()
        {
            
        }

        public override void SwitchTo(Scene? oldScene)
        {
            // create a texture atlas
            Texture = new("TextureAtlas1", new(64, 64), new(4, 4))
            {
                Path = "Content/TextureAtlasTest.png",
                Position = new Vector2(200, 200),
            };

            Lightning.Renderer.AddRenderable(Texture);

            Lightning.Renderer.AddRenderable(new TextBlock("Text1", "Texture atlas example", "DebugFont", new(200, 364), Color.Green));
        }

        public override void SwitchFrom(Scene newScene)
        {
            
        }

        public override void Render()
        {
            Debug.Assert(Texture != null);

            Texture.Index = Random.Shared.Next(0, 15);
            Texture.Position = new(200, 200);
            Texture.DrawFrame();
            Texture.Index = Random.Shared.Next(0, 15);
            Texture.Position = new(200, 264);
            Texture.DrawFrame();
        }
    }
}
