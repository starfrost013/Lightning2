﻿
namespace LightningGL
{
    /// <summary>
    /// Client
    /// 
    /// The Lightning client.
    /// </summary>
    public class Client : LightningCore
    {
        public NetworkClient NetworkClient { get; set; }

        public Client() : base()
        {
            NetworkClient = new NetworkClient($"Test Client {Random.Shared.Next(100000, 999999)}");
        }

        internal override void Init()
        {
            base.Init();

            Logger.Log("Lightning Client initialising...");

            Logger.Log("Initialising renderer...");
            Renderer = RendererFactory.GetRenderer(GlobalSettings.GraphicsRenderer);
            Debug.Assert(Renderer != null);
            Logger.Log($"Using renderer {Renderer.GetType().Name}");

            if (GlobalSettings.GeneralProfilePerformance)
            {
                Logger.Log("Performance Profiler enabled, initialising profiler...");
                PerformanceProfiler.Start();
            }

            // might want to move scene
            if (Renderer is SdlRenderer)
            {
                // Load the scene manager.
                InitSceneManager(new SdlRendererSettings
                {
                    Position = new Vector2(GlobalSettings.GraphicsPositionX, GlobalSettings.GraphicsPositionY),
                    Size = new Vector2(GlobalSettings.GraphicsResolutionX, GlobalSettings.GraphicsResolutionY),
                    WindowFlags = GlobalSettings.GraphicsWindowFlags,
                    RenderFlags = GlobalSettings.GraphicsRenderFlags,
                    Title = GlobalSettings.GraphicsWindowTitle
                });
            }

            // if scenemanager started successfully, run its main loop
            if (Initialised)
            {
                Logger.Log("Client initialisation successful!", ConsoleColor.Green);
                Initialised = true;
                Main();
            }
        }

        internal override void Main()
        {
            if (NetworkClient.Connected)
            {
                NetworkClient.Main();
            }

            base.Main();
        }

        internal void Connect(string serverIp) => NetworkClient.Connect(serverIp);
    }
}
