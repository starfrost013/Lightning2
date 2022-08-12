using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.Numerics;
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
    public static class SceneManager
    {
        /// <summary>
        /// The list of scenes in the Scene Manager.
        /// </summary>
        public static List<Scene> Scenes { get; private set; }

        /// <summary>
        /// The current scene that is being run.
        /// </summary>
        private static Scene CurrentScene { get; set; }

        /// <summary>
        /// The main window of the application.
        /// </summary>
        public static Window MainWindow { get; set; }

        /// <summary>
        /// Determines if the scene manager is running.
        /// </summary>
        public static bool Running { get; set; }

        static SceneManager()
        {
            Scenes = new List<Scene>();
        }

        /// <summary>
        /// Initialises the Scene Manager.
        /// </summary>
        /// <param name="windowSettings">The window settings to use for the Scene Manager.</param>
        /// <exception cref="NCException">An error occurred initialising the Scene Manager.</exception>
        public static void Init(WindowSettings windowSettings)
        {
            // Initialise Lightning2 window.
            MainWindow = new Window();

            MainWindow.Start(windowSettings);

            // Initialise the scenes.
            Assembly assembly = Assembly.GetEntryAssembly();

            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Scene)))
                {
                    Scene scene = (Scene)Activator.CreateInstance(t);

                    if (scene == null) throw new NCException($"Error initialising SceneManager: failed to initialise scene {scene.GetName()}, failed to create instance!", 1000, "Scene initialisation failed in SceneManager::Init", NCExceptionSeverity.FatalError);

                    Scenes.Add(scene);

                    NCLogging.Log($"Initialising scene {scene.GetName()}...");

                    scene.Start();

                    if (GlobalSettings.StartupScene == t.Name) CurrentScene = scene;
                }
            }


            if (Scenes.Count == 0) throw new NCException($"There are no scenes defined!", 1002, "SceneManager::Scenes Count = 0!", NCExceptionSeverity.FatalError);
            if (CurrentScene == null) throw new NCException($"There must be a startup scene set!", 1001, "No current scene specified!", NCExceptionSeverity.FatalError);

            Running = true;
        }

        /// <summary>
        /// Internal: Main loop for the Scene Manager.
        /// </summary>
        internal static void Main()
        {
            while (MainWindow.Run())
            {
                // Only put events you want to GLOBALLY handle here.
                // Every other event will be handled by scene.render
                if (MainWindow.EventWaiting)
                {
                    switch (MainWindow.LastEvent.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            NCLogging.Log("Shutting down scene...");
                            foreach (Scene scene in Scenes)
                            {
                                // shutdown every scene
                                NCLogging.Log($"Shutting down scene {scene.GetName()}...");
                                scene.Shutdown();
                            }

                            // shut down the engine
                            Lightning.Shutdown(MainWindow);

                            // TEMP code
                            return;
                            // END TEMP code
                    }
                }

                // Render the current scene.
                CurrentScene.Render(MainWindow);

                MainWindow.Render();
            }
        }

        /// <summary>
        /// Sets the current scene to the scene <see cref="Scene"/>.
        /// </summary>
        /// <param name="newScene">The new <see cref="Scene"/> to set to be the current scene.</param>
        public static void SetCurrentScene(Scene newScene)
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
        public static void SetCurrentScene(string name)
        {
            Scene scene = GetScene(name);

            if (scene == null) _ = new NCException($"Tried to set invalid scene {name}!", 1003, "Called SceneManager::GetCurrentScene with an invalid scene name");

            SetCurrentScene(scene);
        }

        /// <summary>
        /// Gets the current scene.
        /// </summary>
        /// <param name="name">The <see cref="Scene.Name"/> of the scene</param>
        /// <returns>A <see cref="Scene"/> object containing the first scene with the name <see cref="name"/>, or <c>null</c> if there is no scene by that name.</returns>
        public static Scene GetScene(string name)
        {
            foreach (Scene scene in Scenes)
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