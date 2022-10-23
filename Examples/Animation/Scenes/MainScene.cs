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
            PrimitiveRenderer.DrawText(cWindow, "Hello World!", new Vector2(300, 300), Color.Red);
        }
    }
}
