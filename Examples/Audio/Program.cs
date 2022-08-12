using LightningGL; // use lightninggl
using static NuCore.SDL2.SDL; // import for sdl
using System.Drawing; // for color
using System.Numerics;

//Lightning Audio Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// supported formats: flac, ogg, mod, mp3, midi
// positional audio and volume controls are also supported

AudioManager.LoadFile(@"Content\deepbluecalm.flac", "deepblue");
AudioManager.LoadFile(@"Content\projectx.mod", "projectx");
AudioManager.LoadFile(@"Content\royksopp.mp3", "royksopp");

// get the audio files
AudioFile deepBlue = AudioManager.GetFileWithName("deepblue");
AudioFile projectX = AudioManager.GetFileWithName("projectx");
AudioFile royksopp = AudioManager.GetFileWithName("royksopp");

while (window.Run())
{
    if (window.EventWaiting)
    {
        switch (window.LastEvent.type)
        {
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:

                MouseButton button = (MouseButton)window.LastEvent.button;

                switch (button.Button)
                {
                    case SDL_MouseButton.Left:
                        deepBlue.Play();
                        projectX.Stop();
                        royksopp.Stop();
                        break;
                    case SDL_MouseButton.Right:
                        projectX.Play();
                        deepBlue.Stop();
                        royksopp.Stop();
                        break;
                    case SDL_MouseButton.Middle:
                        royksopp.Play();
                        projectX.Stop();
                        deepBlue.Stop();
                        break;
                }

                break;
        }
    }

    PrimitiveRenderer.DrawText(window, "Audio example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "Left mouse to play audio 1", new Vector2(100, 120), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "Right mouse to play audio 2", new Vector2(100, 140), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "Middle mouse to play audio 3", new Vector2(100, 160), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "Sorry laptop users", new Vector2(100, 180), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}