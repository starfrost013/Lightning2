﻿using NuCore.SDL2;
using System;

namespace LightningGL
{
    /// <summary>
    /// Scene
    /// 
    /// Written March 13, 2022
    /// Integrated into main engine tree and updated for new engine versions August 10, 2022
    /// 
    /// Defines a scene in LightningGL.
    /// A scene is a single "part" of a game.
    /// </summary>
    public abstract class Scene
    {
        internal string Name { get; set; }

        /// <summary>
        /// Run immediately after the scene is initialised, during the initialisation of the Scene Manager after SDL and Lightning2 have been started.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Run each frame
        /// </summary>
        public abstract void Render(Window cWindow);

        /// <summary>
        /// Run when this scene is about to be switched to.
        /// </summary>
        /// <param name="oldScene">The last scene that has been run.</param>
        public abstract void SwitchTo(Scene oldScene);

        /// <summary>
        /// Run when this scene is about to be switched away from.
        /// </summary>
        /// <param name="newScene">The new scene that is about to be switched to.</param>
        public abstract void SwitchAway(Scene newScene);

        /// <summary>
        /// Ran on shutdown - after the window's SDL_QUIT event has been handled
        /// </summary>
        public abstract void Shutdown();

        /// <summary>
        /// Gets the name of this scene.
        /// </summary>
        public virtual string GetName()
        {
            if (Name != null) return Name;

            // if the user didn't specify a name then use the type name
            Type t = GetType();
            string typeName = t.Name;
            return typeName;
        }

        /// <summary>
        /// Determines if this scene is the startup scene. If true, this scene will be initialised at engine start.
        /// There must be at least one startup scene.
        /// 
        /// If multiple scenes are set as the startup scene, the first one set will be used.
        /// </summary>
        public virtual bool StartupScene => false;

        public Scene(string name = null)
        {
            Name = name;
        }
    }
}
