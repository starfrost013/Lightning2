using System.Reflection;

namespace LightningGL
{
    /// <summary>
    /// SceneManager
    /// 
    /// March 14, 2022
    /// 
    /// Defines the Scene Manager, which manages scenes in Lightning (optional)
    /// </summary>
    public class SceneAssetManager : AssetManager<Scene>
    {
        /// <summary>
        /// The current scene that is being run.
        /// </summary>
        private Scene CurrentScene { get; set; }

        /// <summary>
        /// The main renderer of the application.
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// Determines if the scene manager is running.
        /// </summary>
        public bool Initialised { get; private set; }

        public override Scene AddAsset(Renderer cRenderer, Scene asset)
        {
            _ = new NCException("AddAsset not implemented for SceneManager", 160, "SceneAssetManager::AddAsset called", NCExceptionSeverity.FatalError);
            return null;
        }

        /// <summary>
        /// Initialises the Scene Manager.
        /// </summary>
        /// <param name="windowSettings">The window settings to use for the Scene Manager.</param>
        /// <exception cref="NCException">An error occurred initialising the Scene Manager.</exception>
        internal void Init(RendererSettings windowSettings)
        {
            // Initialise Lightning2 window.
            Renderer = new Renderer();

            Renderer.Start(windowSettings);

            // Initialise the scenes.
            Assembly assembly = Assembly.GetEntryAssembly();

            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Scene)))
                {
                    Scene scene = (Scene)Activator.CreateInstance(t);

                    if (scene == null) throw new NCException($"Error initialising SceneManager: failed to initialise scene {scene.GetName()}, failed to create instance!", 130, "Scene initialisation failed in SceneManager::Init", NCExceptionSeverity.FatalError);

                    Assets.Add(scene);

                    NCLogging.Log($"Initialising scene {scene.GetName()}...");

                    scene.Start();

                    if (GlobalSettings.StartupScene == t.Name) CurrentScene = scene;
                }
            }


            if (Assets.Count == 0) throw new NCException($"There are no scenes defined!", 131, "SceneManager::Scenes Count = 0!", NCExceptionSeverity.FatalError);
            if (CurrentScene == null) throw new NCException($"Attempted to specify invalid startup scene {CurrentScene}!", 132, 
                $"The value of GlobalSettings::StartupScene did not correspond to a class that inherits from Scene in the game assembly", NCExceptionSeverity.FatalError);

            Initialised = true;
        }

        /// <summary>
        /// Internal: Main loop for the Scene Manager.
        /// </summary>
        internal void Main()
        {
            while (Renderer.Run())
            {
                // Only put events you want to GLOBALLY handle here.
                // Every other event will be handled by scene.render
                if (Renderer.EventWaiting)
                {
                    switch (Renderer.LastEvent.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            NCLogging.Log("Shutting down scene...");
                            foreach (Scene scene in Assets)
                            {
                                // shutdown every scene
                                NCLogging.Log($"Shutting down scene {scene.GetName()}...");
                                scene.Shutdown();
                            }

                            // shut down the engine
                            Shutdown(Renderer);

                            break;
                    }
                }

                // Render the current scene.
                CurrentScene.Render(Renderer);

                Renderer.Render();
            }
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public void SetCurrentScene(Scene newScene)
        {
            NCLogging.Log($"Setting scene to {newScene.GetName()}...");
            CurrentScene.SwitchAway(newScene);
            newScene.SwitchTo(CurrentScene);
            CurrentScene = newScene;
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public void SetCurrentScene(string name)
        {
            Scene scene = GetScene(name);

            if (scene == null) _ = new NCException($"Tried to set invalid scene {name}!", 133, "Called SceneManager::GetCurrentScene with an invalid scene name");

            SetCurrentScene(scene);
        }

        /// <summary>
        /// Gets the current scene.
        /// </summary>
        /// <param name="name">The <see cref="Scene.Name"/> of the scene</param>
        /// <returns>A <see cref="Scene"/> object containing the first scene with the name <paramref name="name"/>, or <c>null</c> if there is no scene by that name.</returns>
        public Scene GetScene(string name)
        {
            foreach (Scene scene in Assets)
            {
                if (scene.GetName() == name)
                {
                    return scene;
                }
            }

            return null;
        }
    }
}