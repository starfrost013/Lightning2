// see globalusings.cs for namespaces used here

namespace BattleEngine
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

            // Write some

            List<Creature> creatureList = new()
            {
                new Creature("TEST", 0, "its a test", "none lol", 100)
            };

            List<Move> moveList = new()
            {
                new Move("", 1, 140)
            };

            string creatureListString = JsonConvert.SerializeObject(creatureList);
            string moveListString = JsonConvert.SerializeObject(moveList);

            if (!Directory.Exists(@"Content\Data\Creatures")) Directory.CreateDirectory(@"Content\Data\Creatures");
            if (!Directory.Exists(@"Content\Data\Moves")) Directory.CreateDirectory(@"Content\Data\Moves");

            File.WriteAllText(@"Content\Data\Creatures\Creatures.json", creatureListString);
            File.WriteAllText(@"Content\Data\Moves\Moves.json", moveListString);

            GameInstance.Load();
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
