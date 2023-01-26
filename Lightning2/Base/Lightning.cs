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

        public static Server Server { get; private set; }

        #region Asset managers (these are here for compatibility)

        public static Renderer Renderer
        {
            get
            {
                if (Client.Initialised)
                {
                    return Client.Renderer;
                }
                else
                {
                    return Server.Renderer;
                }
            }
        }

        public static Scene? CurrentScene
        {
            get
            {
                if (Client.Initialised)
                {
                    return Client.CurrentScene;
                }
                else
                {
                    return Server.CurrentScene;
                }
            }
        }

        public static TextureAssetManager TextureManager
        {
            get
            {
                if (Client.Initialised)
                {
                    return Client.TextureManager;
                }
                else
                {
                    return Server.TextureManager;
                }
            }
        }

        public static FontAssetManager FontManager
        {
            get
            {
                if (Client.Initialised)
                {
                    return Client.FontManager;
                }
                else
                {
                    return Server.FontManager;
                }
            }
        }

        public static LightAssetManager LightManager
        {
            get
            {
                if (Client.Initialised)
                {
                    return Client.LightManager;
                }
                else
                {
                    return Server.LightManager;
                }
            }
        }

        #endregion

        static Lightning()
        {
            Client = new Client();
            Server = new Server();

            // Set culture to invariant so things like different decimal symbols don't crash
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            NCLogging.Settings.WriteToLog = true;
            NCLogging.Init();
        }

        public static void InitClient()
        {
            try
            {
                Client.Init();
            }
            catch (Exception err)
            {
                NCLogging.LogError($"An unknown fatal error occurred during client initialisation. The installation may be corrupted!",
                    0x0000DEAD, NCLoggingSeverity.FatalError, err);
            }
        }

        public static void InitServer()
        {
            try
            {
                Server.Init();
            }
            catch (Exception err)
            {
                NCLogging.LogError($"An unknown fatal error occurred during server initialisation. The installation may be corrupted!",
                    0x0001DEAD, NCLoggingSeverity.FatalError, err);
            }
        }

        public static void Shutdown()
        {
            if (Client.Initialised)
            {
                Client.Shutdown();
            }
            else
            {
                Server.Shutdown();
            }
        }

        public static void SetCurrentScene(string sceneName)
        {
            if (Client.Initialised)
            {
                Client.SetCurrentScene(sceneName);
            }
            else
            {
                Server.SetCurrentScene(sceneName);
            }
        }

        public static void SetCurrentScene(Scene scene)
        {
            if (Client.Initialised)
            {
                Client.SetCurrentScene(scene);
            }
            else
            {
                Server.SetCurrentScene(scene);
            }
        }
    }
}