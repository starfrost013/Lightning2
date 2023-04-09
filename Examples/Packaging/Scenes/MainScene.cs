
namespace BasicScene
{
    public class MainScene : Scene
    {
        private Texture Coll1 { get; set; }

        private Texture Coll2 { get; set; }

        public override void Start()
        {
        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene? oldScene)
        {

            // draw textures
            Coll1 = new("Test1", 128, 128, false, "Content/CollidingTexture1.pnbg")
            {
                Path = "Content/CollidingTexture1.png",
                Position = new Vector2(128, 300),
            };

            Coll2 = new("Test2", 128, 128, false, "Content/CollidingTexture2.png")
            {
                Path = "Content/CollidingTexture2.png",
                Position = new Vector2(Lightning.Renderer.Settings.Size.X - 128, 300),
            };

            Lightning.Tree.AddRenderable(Coll1);
            Lightning.Tree.AddRenderable(Coll2);

            // draw text
            Lightning.Tree.AddRenderable(new TextBlock("Text3", "Collision/AABB example (NO correction is being done here) - loaded from package file", "DebugFont", new Vector2(100, 100), Color.White)); // no fonts loaded so we use the debug font
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {
            if (!AABB.Intersects(Coll1, Coll2))
            {
                Coll1.Position = new Vector2(Coll1.Position.X + (0.1f * (float)Lightning.Renderer.DeltaTime), Coll1.Position.Y);
                Coll2.Position = new Vector2(Coll2.Position.X - (0.1f * (float)Lightning.Renderer.DeltaTime), Coll2.Position.Y);
            }
        }
    }
}
