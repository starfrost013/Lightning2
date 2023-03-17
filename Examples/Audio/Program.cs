
//Lightning Audio Example for Lightning 2.0; 
//©2022-2023 starfrost, March 14, 2023 

// Initialise Lightning
Lightning.InitClient();

// supported formats: flac, ogg, mp3, midi (mods are broken)
// positional audio and volume controls are also supported

AudioManager.LoadFile(renderer, @"Content\deepbluecalm.flac", "deepblue");
//AudioManager.LoadFile(renderer, @"Content\projectx.mod", "projectx");
AudioManager.LoadFile(renderer, @"Content\royksopp.mp3", "royksopp");

// get the audio files
AudioFile deepBlue = AudioManager.GetFileWithName("deepblue");
//AudioFile projectX = AudioManager.GetFileWithName("projectx");
AudioFile royksopp = AudioManager.GetFileWithName("royksopp");

while (renderer.Run())
{
    if (renderer.EventWaiting)
    {
        switch (renderer.LastEvent.type)
        {
            case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                MouseButton button = (MouseButton)renderer.LastEvent.button;

                switch (button.Button)
                {
                    case SDL_MouseButton.Left:
                        deepBlue.Play();
                        //projectX.Stop();
                        royksopp.Stop();
                        break;
                    case SDL_MouseButton.Right:
                        //projectX.Play();
                        deepBlue.Stop();
                        royksopp.Stop();
                        break;
                    case SDL_MouseButton.Middle:
                        royksopp.Play();
                        //projectX.Stop();
                        deepBlue.Stop();
                        break;
                }

                break;
        }
    }

    PrimitiveRenderer.DrawText(renderer, "Audio example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(renderer, "Left mouse to play audio 1", new Vector2(100, 120), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(renderer, "Right mouse to play audio 2", new Vector2(100, 140), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(renderer, "Middle mouse to play audio 3", new Vector2(100, 160), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(renderer, "Sorry laptop users", new Vector2(100, 180), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}