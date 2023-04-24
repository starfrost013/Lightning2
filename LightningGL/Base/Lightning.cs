namespace LightningGL
{
    /// <summary>
    /// Lightning
    /// 
    /// March 14, 2022
    /// 
    /// Defines the Scene Manager, which manages scenes in Lightning (optional)
    /// </summary>
    public static class Lightning
    {
        public static Client Client { get; private set; }

        private static bool Initialised { get; set; }

        #region Core Content 

        public static Renderer Renderer => Client.Renderer;

        public static Scene? CurrentScene => Client.CurrentScene;

        public static RenderTree Tree => Client.Tree;

        public static LightAssetManager LightManager => Client.LightManager;

        #endregion

        static Lightning()
        {
            Client = new Client();

            // Set culture to invariant so things like different decimal symbols don't crash
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Logger.Settings.WriteToLog = true;
            Logger.Init();
        }

        [RequiresPreviewFeatures]
        public static void InitClient()
        {
            try
            {
                if (Initialised)
                {
                    Logger.LogError($"Attempted to initialise after initialisation!", 400, LoggerSeverity.Warning, null, true);
                }

                Client.Init();

                Initialised = true;
            }
            catch (Exception err)
            {
                Logger.LogError($"An unknown fatal error occurred during client initialisation. The installation may be corrupted!",
                    0x0000DEAD, LoggerSeverity.FatalError, err);
            }
        }

        public static void Shutdown() => Client.Shutdown();

        public static void SetCurrentScene(string sceneName) => Client.SetCurrentScene(sceneName);

        public static void SetCurrentScene(Scene scene) => Client.SetCurrentScene(scene);
    }
}