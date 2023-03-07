namespace BattleEngine
{
    internal class MoveManager
    {
        /// <summary>
        /// The internal move list
        /// </summary>
        internal List<Move> Moves { get; set; }

        private const string MOVE_JSON_FILE = @"Content\Data\Moves\Moves.json";
        internal MoveManager()
        {
            Moves = new();
        }   

        internal bool LoadMoves()
        {
            if (!Uri.IsWellFormedUriString(MOVE_JSON_FILE, UriKind.RelativeOrAbsolute)
                || !File.Exists(MOVE_JSON_FILE))
            {
                Logger.LogError($"Invalid moves.json location ({MOVE_JSON_FILE}). Please fix!", 1800, LoggerSeverity.FatalError);
                return false;
            }

            try
            {
                List<Move>? move = JsonConvert.DeserializeObject<List<Move>>(MOVE_JSON_FILE);

                if (move == null)
                {
                    Logger.LogError($"Failed to load Moves.json!", 1801, LoggerSeverity.FatalError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Moves.json!\n\n{ex}", 1802, LoggerSeverity.FatalError);
            }

            return true; 
        }
    }
}
