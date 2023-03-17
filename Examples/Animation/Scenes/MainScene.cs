// see globalusings.cs for namespaces used here

namespace Animation
{
    /// <summary>
    /// MainScene
    /// 
    /// The main scene of your Lightning game. 
    /// Add additional scenes by creating classes that inherit from Scene.
    /// </summary>
    public class MainScene : Scene
    {
        public override void Start()
        {
            // probably shoud not have called the project animation but oh well
            LightningGL.Animation newAnim = new("Anim1", @"Content\Anim.json");
            newAnim = Lightning.Renderer.AddRenderable(newAnim);
            Texture texture = new("CollidingTexture1", 64, 64)
            {
                Path = @"Content\CollidingTexture1.png",
            };

            texture.SetAnimation(newAnim);

            texture.StartCurrentAnimation();

            Lightning.Renderer.AddRenderable(texture);
        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Renderer.AddRenderable(new TextBlock("Animation", "Lightning Animation Example", "DebugFont", new(300, 300), Color.Aquamarine));
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {

        }
    }
}
