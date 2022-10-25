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
            LightningGL.Animation newAnim = new LightningGL.Animation(@"Content\Anim.json");
            newAnim = AnimationManager.AddAsset(SceneManager.Renderer, newAnim);
            Texture texture = new Texture(SceneManager.Renderer, 64, 64, SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING)
            {
                Path = @"Content\CollidingTexture1.png",
            };

            texture.SetAnimation(newAnim);

            texture.StartCurrentAnimation();

            TextureManager.AddAsset(SceneManager.Renderer, texture); // automatically loads
        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {

        }

        public override void SwitchAway(Scene newScene)
        {

        }

        public override void Render(Renderer cWindow)
        {
            PrimitiveRenderer.DrawText(cWindow, "Lightning Animation Example", new Vector2(300, 300), Color.Red);
        }
    }
}
