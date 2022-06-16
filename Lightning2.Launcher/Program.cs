// See https://aka.ms/new-console-template for more information
// Test LightningGL application

// © 2022 starfrost

using LightningGL;
using NuCore.SDL2;
using System.Drawing;
using System.Numerics;

LightningGL.LightningGL.Init(); // Initialise LightningGL
Window window = new Window();
WindowSettings window_settings = new WindowSettings();

window_settings.Title = "LightningGL Example";
window_settings.Position = new Vector2(350, 100);
window_settings.Size = new Vector2(960, 640);

window.Start(window_settings);

FontManager.LoadFont("Arial", 11, null, "Arial.11pt");
FontManager.LoadFont("Arial", 18, null, "Arial.18pt");
FontManager.LoadFont("Arial", 24, null, "Arial.24pt");
FontManager.LoadFont("Arial", 36, null, "Arial.36pt");

window.Clear(Color.FromArgb(255, 127, 127, 127));
Texture texture = new Texture(window, new(64, 64));

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
        texture.SetPixel(x, y, Color.FromArgb(A, R, G, B));
    }
}

texture.Unlock();

texture.Position = new Vector2(0, 0);
texture.Repeat = new Vector2(3, 3);

AtlasTexture atlas_texture = new AtlasTexture();

atlas_texture.Path = @"Content\AtlastextureTest.png";

atlas_texture.FrameSize = new Vector2(64, 64); // size of one tile
atlas_texture.Load(window, 4, 4);
atlas_texture.Position = new Vector2(256, 256);

AnimatedTexture at = new AnimatedTexture();
at.FramesPath.Add(@"Content\AnimTextureTest\AnimtextureTestF0.png");
at.FramesPath.Add(@"Content\AnimTextureTest\AnimtextureTestF1.png");
at.FramesPath.Add(@"Content\AnimTextureTest\AnimtextureTestF2.png");
at.FramesPath.Add(@"Content\AnimTextureTest\AnimtextureTestF3.png");
at.Cycle = new AnimationCycle(0, 3, 60);

at.Position = new Vector2(320, 256);
at.Size = new Vector2(256, 256);

at.Load(window);

LightManager.Init(window);
LightManager.SetEnvironmentalLight(Color.FromArgb(0, 0, 0, 255));

// todo: particleeffectsettings?

Texture testEffectTexture = new Texture(window, new(16, 16));

testEffectTexture.Path = @"Content\Sparkles.png";

ParticleEffect testEffect = new ParticleEffect(testEffectTexture)
{
    Amount = 400,
    Lifetime = 15,
    Variance = 50,
    Velocity = new(4f, -4f),
    Position = new(150, 500)
};

ParticleManager.AddParticleEffect(window, testEffect);

LightManager.AddLight(window, new Light
{
    Position = new Vector2(50, 375),
    Range = 4,
    Brightness = 15,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(250, 300),
    Range = 4,
    Brightness = 31,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(450, 225),
    Range = 4,
    Brightness = 63,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(650, 150),
    Range = 4,
    Brightness = 127,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(850, 75),
    Range = 4,
    Brightness = 255,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(850, 275),
    Range = 4,
    Brightness = 255,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(850, 475),
    Range = 4,
    Brightness = 255,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(850, 675),
    Range = 4,
    Brightness = 255,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(0, 0),
    Range = 4,
    Brightness = 255,
});

LightManager.AddLight(window, new Light
{
    Position = new Vector2(200, 0),
    Range = 4,
    Brightness = 255,
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

Button btn1 = new Button()
{
    Position = new Vector2(150, 150),
    Size = new Vector2(44, 44),
    Text = "button",
    BackgroundColour = Color.PaleGoldenrod,
    HighlightColour = Color.Goldenrod,
    PressedColour = Color.DarkGoldenrod,
    ForegroundColour = Color.Black,
    Filled = true,
    Font = "Arial.11pt"
};

ListBox listBox1 = new ListBox
{
    Position = new Vector2(70, 150),
    Size = new Vector2(44, 44),
    BackgroundColour = Color.PaleGoldenrod,
    HighlightColour = Color.Goldenrod,
    PressedColour = Color.DarkGoldenrod,
    ForegroundColour = Color.Black,
    Filled = true,
    Font = "Arial.11pt"
};

listBox1.AddItem(new ListBoxItem("test 1"));
listBox1.AddItem(new ListBoxItem("h"));
listBox1.AddItem(new ListBoxItem("dfsdfsdfsdfsdfsdf"));
listBox1.AddItem(new ListBoxItem("zxczxzxzx"));
listBox1.AddItem(new ListBoxItem("qasqsdfwqer"));

UIManager.AddElement(btn1);
UIManager.AddElement(listBox1);

while (window.Run())
{
    SDL.SDL_Event cur_event = window.LastEvent;

    if (window.EventWaiting)
    {
        switch (cur_event.type)
        {
            case SDL.SDL_EventType.SDL_KEYDOWN: // Key is held down.
                Key nkey = (Key)cur_event.key;

                string key_string = nkey.ToString();

                nkey.Repeated = (cur_event.key.repeat > 0);

                switch (key_string)
                {
                    case "LEFT":
                    case "A":
                        camera.Position -= new Vector2(10, 0);
                        continue;
                    case "RIGHT":
                    case "D":
                        camera.Position += new Vector2(10, 0);
                        continue;
                    case "UP":
                    case "W":
                        camera.Position -= new Vector2(0, 10);
                        continue;
                    case "DOWN":
                    case "S":
                        camera.Position += new Vector2(0, 10);
                        continue;
                }
                continue;
        }
    }

    PrimitiveRenderer.DrawLine(window, new Vector2(500, 300), new Vector2(600, 300), 1, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 270), new Vector2(600, 270), 3, Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 240), new Vector2(600, 240), 7, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(window, new Vector2(500, 210), new Vector2(600, 210), 15, Color.FromArgb(255, 255, 255, 255), true);

    PrimitiveRenderer.DrawCircle(window, new Vector2(500, 10), new Vector2(50, 50), Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawCircle(window, new Vector2(500, 309), new Vector2(50, 50), Color.FromArgb(127, 255, 255, 255), false);

    PrimitiveRenderer.DrawRectangle(window, new Vector2(552, 10), new Vector2(30, 30), Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawRectangle(window, new Vector2(584, 10), new Vector2(30, 30), Color.FromArgb(33, 0, 0, 255), true);

    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(616, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, false);
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(648, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, true);
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(680, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 12, true);

    PrimitiveRenderer.DrawTriangle(window, new Vector2(722, 10), new Vector2(747, 40), new Vector2(707, 40), Color.FromArgb(127, 255, 255, 255), false);
    PrimitiveRenderer.DrawTriangle(window, new Vector2(779, 10), new Vector2(794, 40), new Vector2(764, 40), Color.FromArgb(127, 255, 255, 255), true);

    PrimitiveRenderer.DrawText(window, "#[STRING_TEST]", new Vector2(500, 90), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(window, "Loc string test: #[STRING_TEST]", new Vector2(500, 120), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(window, "Loc string test: #[STRING_TEST] aaaaaa #[STRING_TEST] #[STRING_TEST] bbbbbb", new Vector2(500, 150), Color.FromArgb(255, 0, 0, 255));

    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 10), Color.FromArgb(255, 255, 255, 255));
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 30), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 50), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Italic);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 70), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Underline);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 90), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Strikeout, 15);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 110), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, 3);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 130), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, 15, -1);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 150), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, 30);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 170), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, 0, FontSmoothingType.Shaded);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 190), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, 0, FontSmoothingType.Solid);
    FontManager.DrawText(window, "Test", "Arial.11pt", new Vector2(700, 210), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
    FontManager.DrawText(window, "#[STRING_TEST]\nTest2\nTest3", "Arial.11pt", new Vector2(700, 230), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline | SDL_ttf.TTF_FontStyle.Strikeout);

    atlas_texture.Position = new Vector2(256, 256);
    atlas_texture.Index = 0;
    atlas_texture.DrawFrame(window);
    atlas_texture.Index = 0;
    atlas_texture.Position = new Vector2(256, 192);
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
            texture.SetPixel(x, y, Color.FromArgb(A, R, G, B));
        }
    }

    at.DrawCurrentFrame(window);

    texture.Unlock();
    texture.Draw(window);

    window.Render();
}