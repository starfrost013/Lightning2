// See GlobalUsings.cs for namespaces used here

namespace basic
{
    /// <summary>
    /// This is a scene of your Lightning game.
    /// 
    /// A scene is a specific area of a game with its own main loop. An example use of scenes would be to use one scene for a game's menu and another scene
    /// for actually playing the game.
    /// </summary>
    public class TestScene : Scene
    {
        public override void Start()
        {

        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene? oldScene)
        {
            Lightning.Renderer.AddRenderable(new TextBlock("TextBlock1", "Hello World!", "DebugFont", new Vector2(300, 300), Color.Red));
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {

        }
    }
}
