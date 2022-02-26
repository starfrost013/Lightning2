using Lightning.Core.API; 
using NuCore.NativeInterop;
#if WINDOWS 
using NuCore.NativeInterop.Win32;
#endif
using NuCore.Utilities;
using NuRender;
using NuRender.SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace Lightning.Core
{
    /// <summary>
    /// BootWindow
    /// 
    /// January 2, 2022
    /// 
    /// Defines the Lightning boot progress window. Replaces SplashScreen
    /// 
    /// Creates a special SDL window (using SDL2 directly) instead of calling through NuRender.
    /// </summary>
    public class BootWindow
    {
        private double _progress { get; set; }

        private Scene BWScene { get; set; }

        private NuRender.Text CurrentProgressString { get; set; }

        private NuRender.Font CurrentProgressFont { get; set;  }
        public BootWindow()
        {
            BWScene = new Scene();
        }

        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (value < 0.0) value = 0.0;
                if (value > 1.0) value = 1.0;
                _progress = value;
                
            }

        }
        
        /// <summary>
        /// Determines if the bootwindow loaded successfully. As BootWindow is not part of the datamodel it cannot unload itself.
        /// </summary>
        private bool Loaded { get; set; }

        public bool Spun { get; set; }

        public void SetProgress(double NProgress, string NProgressString)
        {
            Progress = NProgress;
            CurrentProgressString.Content = NProgressString;
            int SizeX, SizeY;
            SDL_ttf.TTF_SizeText(CurrentProgressFont.Pointer, CurrentProgressString.Content, out SizeX, out SizeY);

            CurrentProgressString.Position = new Vector2Internal(960 / 2 - (SizeX / 2), CurrentProgressString.Position.Y);

            
            if (!Spun) // worst hack
            {
                Update(true);
                Spun = true; 
            }
            else
            {
                Update(false);
            }
        }

        public void Init() // result class?
        {
            // no nurender, so create a window
            // (this runs pre-init)

            // only initialise the subsystems we need to reduce singlethreaded loading time
            SDL.SDL_Init(SDL.SDL_InitFlags.SDL_INIT_EVENTS | SDL.SDL_InitFlags.SDL_INIT_VIDEO);
            SDL_ttf.TTF_Init();
#if WINDOWS
            BWScene.AddWindow(new WindowSettings { ApplicationName = "BootWindow (temporary)",
                IsMainWindow = true,
                WindowSize = new Vector2Internal(960, 480),
                WindowPosition = new Vector2Internal(NativeMethodsWin32.GetSystemMetrics(SystemMetric.SM_CXSCREEN) / 2 - (960 / 2), NativeMethodsWin32.GetSystemMetrics(SystemMetric.SM_CYSCREEN) / 2 - (480 / 2)),
                WindowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN });
#else
                Logging.Log("BootWindow not implemented on Linux/OSX yet!");
#endif


            Init_LoadBootSplash();
            Init_LoadText();

            Update();

            Loaded = true;
            return;
        }

        private void Init_LoadBootSplash()
        {
            GlobalSettings GS = DataModel.GetGlobalSettings();

            if (File.Exists(GS.BootSplashPath))
            {
                Window MainWindow = BWScene.GetMainWindow();

                Image NRImage = (Image)(MainWindow.AddObject("Image"));
                NRImage.TextureInfo.Path = GS.BootSplashPath;
                NRImage.Size = new Vector2Internal(960, 480);
                NRImage.Load(MainWindow.Settings.RenderingInformation);
            }
            else
            {
                ErrorManager.ThrowError("BootWindow", $"Cannot find {GS.BootSplashPath}!");
                return; 
            
            }
            
        }

        private void Init_LoadText()
        {
            Window MainWindow = BWScene.GetMainWindow();
            CurrentProgressFont = (NuRender.Font)MainWindow.AddObject("Font");
            CurrentProgressFont.Name = "Arial";
            CurrentProgressFont.Size = 20;
            CurrentProgressFont.Load(MainWindow.Settings.RenderingInformation);
            CurrentProgressString = (NuRender.Text)MainWindow.AddObject("Text");

            CurrentProgressString.Position = new Vector2Internal(250, 360);
            CurrentProgressString.BackgroundColour = new Color4Internal(127, 255, 0, 0);
            CurrentProgressString.Colour = new Color4Internal(255, 255, 255, 255);
            CurrentProgressString.Font = "Arial";
            return; 
            
        }

        private void Update(bool Spin = false) // only render when required
        {
            if (!Loaded) return;

            // Giant hack.
            // SDL takes X cycles to bring up the window and properly render it.
            // We will use 20 here

            if (Spin)
            {
                for (int i = 0; i < 20; i++)
                {
                    BWScene.Render();
                }
            }
            else
            {
                BWScene.Render();
            }
           
        }

        public void Shutdown()
        {
            Window MainWindow = BWScene.GetMainWindow();

            BWScene.ShutdownWindowWithID(MainWindow.Settings.WindowID);
            return;
        }

    }
}
