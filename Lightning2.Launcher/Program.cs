// See https://aka.ms/new-console-template for more information
// Test Lightning2 application

// © 2022 starfrost

using NuCore.SDL2;
using NuCore.Utilities;
using Lightning2;
using System.Numerics; 

Lightning2.Lightning2.Init(); // Initialise Lightning2
Window window = new Window();
WindowSettings window_settings = new WindowSettings();

window_settings.Title = "Lightning2 Example";
window_settings.Position = new Vector2(350, 100);
window_settings.Size = new Vector2(960, 640);

window.Start(window_settings);

TextManager.LoadFont("Arial", 11, null, "Arial.11pt");
TextManager.LoadFont("Arial", 18, null, "Arial.18pt");
TextManager.LoadFont("Arial", 24, null, "Arial.24pt");
TextManager.LoadFont("Arial", 36, null, "Arial.36pt");

window.Clear(new Color4(127, 127, 127, 255));
Texture texture = new Texture(window, 64, 64);

Random rnd = new Random();

// Texture API test
byte R = (byte)rnd.Next(0, 256);
byte G = (byte)rnd.Next(0, 256);
byte B = (byte)rnd.Next(0, 256);
byte A = (byte)rnd.Next(0, 256);

for (int x = 0; x < texture.Size.X; x++)
{
    B += (byte)rnd.Next(-5, 5);
    G += (byte)rnd.Next(-5, 5);
    B += (byte)rnd.Next(-5, 5);
    A += (byte)rnd.Next(-5, 5);

    for (int y = 0; y < texture.Size.Y; y++)
    {
        texture.SetPixel(x, y, new Color4(R, G, B, A));
    }
}

texture.Unlock();

texture.Position = new Vector2(0, 0);
texture.Repeat = new Vector2(3, 3);

AtlasTexture atlas_texture = new AtlasTexture();

atlas_texture.Path = @"Content\AtlastextureTest.png";
atlas_texture.Position = new Vector2(256, 256);
atlas_texture.FrameSize = new Vector2(64, 64); // size of one tile
atlas_texture.Load(window, 4, 4);

bool Running = true;

AnimatedTexture at = new AnimatedTexture();
at.FramesPath.Add(@"Content\AnimtextureTest\AnimtextureTestF0.png");
at.FramesPath.Add(@"Content\AnimtextureTest\AnimtextureTestF1.png");
at.FramesPath.Add(@"Content\AnimtextureTest\AnimtextureTestF2.png");
at.FramesPath.Add(@"Content\AnimtextureTest\AnimtextureTestF3.png");
at.Cycle = new AnimationCycle(0, 3, 60);

at.Position = new Vector2(320, 256);
at.Size = new Vector2(256, 256);

at.Load(window);

LightManager.Init(window);
LightManager.SetEnvironmentalLight(new Color4(255, 0, 0, 255));

//LightManager.AddLight(new Light
//{
//    Position = new Vector2(-300, 200),
//    Brightness = 4
//});

//LightManager.AddLight(new Light
//{
//    Position = new Vector2(-300, 300),
//    Brightness = 4
//});

LightManager.AddLight(new Light
{
    Position = new Vector2(850, 75),
    Brightness = 50,
    SnapToScreen = true
});

AudioManager.LoadFile(@"Content\xm_boot.mp3");
AudioManager.LoadFile(@"Content\xm_boot_ogg.ogg");
AudioManager.LoadFile(@"Content\xm_title.mp3");

AudioFile xm_boot = AudioManager.GetFileWithName("xm_boot");
AudioFile xm_boot_ogg = AudioManager.GetFileWithName("xm_boot_ogg");
AudioFile xm_title = AudioManager.GetFileWithName("xm_title");

xm_boot.Play();
xm_boot_ogg.Play();

xm_title.SetVolume(1);
xm_title.Repeat = -1;
xm_title.PositionalSound = true; 
xm_title.Play();

Camera camera = new Camera(CameraType.Chase);

window.SetCurrentCamera(camera);

while (Running)
{
    window.Update();

    SDL.SDL_Event cur_event = new SDL.SDL_Event();

    if (SDL.SDL_PollEvent(out cur_event) > 0)
    {
        switch (cur_event.type)
        {
            case SDL.SDL_EventType.SDL_QUIT: // User requested a quit.
                Running = false; // shut down
                continue;
            case SDL.SDL_EventType.SDL_KEYDOWN: // Key is held down.
                Key nkey = (Key)cur_event.key;

                string key_string = nkey.ToString();

                nkey.Repeated = (cur_event.key.repeat > 0);
                NCLogging.Log($"KeyPress: {key_string}, repeated: {nkey.Repeated}");

                switch (key_string)
                {
                    case "LEFT":
                    case "A":
                        camera.Position += new Vector2(-10, 0);
                        continue;
                    case "RIGHT":
                    case "D":
                        camera.Position += new Vector2(10, 0);
                        continue;
                    case "UP":
                    case "W":
                        camera.Position += new Vector2(0, -10);
                        continue;
                    case "DOWN":
                    case "S":
                        camera.Position += new Vector2(0, 10);
                        continue;
                }
                continue; // TEMP Code
        }
    }

    PrimitiveRenderer.DrawLine(window, new Vector2(500, 300), new Vector2(600, 300), 1, new Color4(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 270), new Vector2(600, 270), 3, new Color4(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 240), new Vector2(600, 240), 7, new Color4(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 210), new Vector2(600, 210), 15, new Color4(255, 255, 255, 255), true);

    PrimitiveRenderer.DrawCircle(window, new Vector2(500, 10), new Vector2(50, 50), new Color4(255, 0, 0, 255), true);
    PrimitiveRenderer.DrawCircle(window, new Vector2(500, 309), new Vector2(50, 50), new Color4(255, 255, 255, 127), false) ;

    PrimitiveRenderer.DrawRectangle(window, new Vector2(552, 10), new Vector2(30, 30), new Color4(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawRectangle(window, new Vector2(584, 10), new Vector2(30, 30), new Color4(255, 0, 0, 127), true);

    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(616, 10), new Vector2(30, 30), new Color4(255, 255, 255, 127), 3, false);
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(648, 10), new Vector2(30, 30), new Color4(255, 255, 255, 127), 3, true);
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(680, 10), new Vector2(30, 30), new Color4(255, 255, 255, 127), 12, true);

    PrimitiveRenderer.DrawTriangle(window, new Vector2(722, 10), new Vector2(747, 40), new Vector2(707, 40), new Color4(255, 255, 255, 127), false);
    PrimitiveRenderer.DrawTriangle(window, new Vector2(779, 10), new Vector2(794, 40), new Vector2(764, 40), new Color4(255, 255, 255, 127), true);

    PrimitiveRenderer.DrawText(window, "#{STRING_TEST}", new Vector2(500, 90), new Color4(0, 0, 255, 255));
    PrimitiveRenderer.DrawText(window, "Loc string test: #{STRING_TEST}", new Vector2(500, 120), new Color4(0, 0, 255, 255));
    PrimitiveRenderer.DrawText(window, "Loc string test: #{STRING_TEST} aaaaaa #{STRING_TEST} #{STRING_TEST} bbbbbb", new Vector2(500, 150), new Color4(0, 0, 255, 255));

    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 10), new Color4(255, 255, 255, 255));
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 30), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Bold);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 50), new Color4(255, 255, 0, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Italic);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 70), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Underline);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 90), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Strikeout, 15);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 110), new Color4(0, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Bold, -1, 3);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 130), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Bold, 15, -1);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 150), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255),  SDL_ttf.TTF_FontStyle.Bold, -1, -1, 30);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 170), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255),  SDL_ttf.TTF_FontStyle.Bold, -1, -1, 0, true, FontSmoothingType.Shaded);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 190), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255), SDL_ttf.TTF_FontStyle.Bold, -1, -1, 0, true, FontSmoothingType.Solid);
    TextManager.DrawTextTTF(window, "Test", "Arial.11pt", new Vector2(700, 210), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255),  SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
    TextManager.DrawTextTTF(window, "#{STRING_TEST}\nTest2\nTest3", "Arial.11pt", new Vector2(700, 230), new Color4(255, 255, 255, 255), new Color4(255, 0, 0, 255),  SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline | SDL_ttf.TTF_FontStyle.Strikeout);

    atlas_texture.Index = new Vector2(3, 1);
    atlas_texture.DrawFrame(window);

    xm_title.Update(window);
    
    for (int x = 0; x < texture.Size.X; x++)
    {
        // textureAPI test
        B += (byte)rnd.Next(-1, 1);
        G += (byte)rnd.Next(-1, 1);
        B += (byte)rnd.Next(-1, 1);
        A += (byte)rnd.Next(-1, 1);

        for (int y = 0; y < texture.Size.Y; y++)
        {
            texture.SetPixel(x, y, new Color4(R, G, B, A));
        }
    }

    at.DrawCurrentFrame(window);

    texture.Unlock();
    texture.Draw(window);

    window.Present();
}

// we're done running so shutdown
Lightning2.Lightning2.Shutdown(window);