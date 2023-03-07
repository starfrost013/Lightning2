namespace BattleEngine
{
    internal class CreatureManager
    {
        /// <summary>
        /// The internal move list
        /// </summary>
        internal List<Creature> Creatures { get; set; }

        private const string MOVE_JSON_FILE = @"Content\Data\Creatures\Creatures.json";

        internal CreatureManager()
        {
            Creatures = new();
        }   

        internal bool LoadCreatures()
        {
            if (!Uri.IsWellFormedUriString(MOVE_JSON_FILE, UriKind.RelativeOrAbsolute)
                || !File.Exists(MOVE_JSON_FILE))
            {
                Logger.LogError($"Invalid moves.json location ({MOVE_JSON_FILE}). Please fix!", 1800, LoggerSeverity.FatalError);
                return false;
            }

            try
            {
                List<Creature>? creatureList = JsonConvert.DeserializeObject<List<Creature>>(MOVE_JSON_FILE);

                if (creatureList == null)
                {
                    Logger.LogError($"Failed to load Moves.json!", 1801, LoggerSeverity.FatalError);
                    return false;
                }

                Creatures = creatureList;
                return true; 
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Moves.json!\n\n{ex}", 1802, LoggerSeverity.FatalError);
                return false;
            }
        }
    }
}
