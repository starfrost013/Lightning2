

namespace BattleEngine
{
    internal static class GameInstance
    {
        internal static CreatureManager CreatureManager { get; set; }

        internal static MoveManager MoveManager { get; set; }

        static GameInstance()
        {
            CreatureManager = new CreatureManager();
            MoveManager = new MoveManager();
        }

        private static void Load()
        {
            Logger.Log("Loading...");
            CreatureManager.LoadCreatures();
            MoveManager.LoadMoves();
        }
    }
}
