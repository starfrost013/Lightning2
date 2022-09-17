// See https://aka.ms/new-console-template for more information
// Test LightningGL application

// © 2022 starfrost

using LightningGL;
using NuCore.SDL2;
using System.Drawing;
using System.Numerics;
using static LightningGL.Lightning;

Init(args); // Initialise LightningGL
Window cWindow = new Window();
WindowSettings windowSettings = new WindowSettings();

windowSettings.Title = "LightningGL Example";
//windowSettings.Position = new Vector2(350, 100);

cWindow.Start(windowSettings);

FontManager.LoadFont("Arial", 11, "Arial.11pt");
FontManager.LoadFont("Arial", 18, "Arial.18pt");
FontManager.LoadFont("Arial", 24, "Arial.24pt");
FontManager.LoadFont("Arial", 36, "Arial.36pt");

cWindow.Clear(Color.FromArgb(255, 127, 127, 127));
Texture texture = new(cWindow, 64, 64);

Random rnd = new();

// Texture API test
byte r = (byte)rnd.Next(0, 256);
byte g = (byte)rnd.Next(0, 256);
byte b = (byte)rnd.Next(0, 256);
byte a = (byte)rnd.Next(0, 256);

for (int x = 0; x < texture.Size.X; x++)
{
    r += (byte)rnd.Next(-5, 5);
    g += (byte)rnd.Next(-5, 5);
    b += (byte)rnd.Next(-5, 5);
    a += (byte)rnd.Next(-5, 5);

    for (int y = 0; y < texture.Size.Y; y++) texture.SetPixel(x, y, Color.FromArgb(a, r, g, b));
}

texture.Unlock();

texture.Position = new Vector2(0, 0);
texture.Repeat = new Vector2(3, 3);

TextureAtlas textureAtlas1 = new(cWindow, new(64, 64), new(4, 4));

textureAtlas1.Path = @"Content\TextureAtlasTest.png";

textureAtlas1.Position = new(256, 256);

textureAtlas1.Load(cWindow);

AnimatedTexture animatedTexture1 = new AnimatedTexture(cWindow, 256, 256, new(0, 3, 1000));
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");

animatedTexture1.Position = new(320, 256);

animatedTexture1.Load(cWindow);

LightManager.SetEnvironmentalLight(Color.FromArgb(0, 0, 0, 255));

// todo: particleeffectsettings?

Texture testEffectTexture = new(cWindow, 16, 16);

testEffectTexture.Path = @"Content\Sparkles.png";

ParticleEffect testEffect = new(testEffectTexture)
{
    Amount = 100,
    Lifetime = 70,
    Variance = 40,
    Velocity = new(0.04f, -0.04f),
    Position = new(150, 500),
    MaxNumberCreatedEachFrame = 1,
    Mode = ParticleMode.AbsoluteVelocity,
};

ParticleManager.AddAsset(cWindow, testEffect);

LightManager.AddAsset(cWindow, new Light
{
    Position = new(50, 375),
    Range = 4,
    Brightness = 15,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(250, 300),
    Range = 4,
    Brightness = 31,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(450, 225),
    Range = 4,
    Brightness = 63,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(650, 150),
    Range = 4,
    Brightness = 127,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(850, 75),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(850, 275),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(850, 475),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(850, 675),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(0, 0),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cWindow, new Light
{
    Position = new(200, 0),
    Range = 4,
    Brightness = 255,
});

AudioManager.LoadFile(@"Content\xm_boot.mp3");
AudioManager.LoadFile(@"Content\xm_boot_ogg.ogg");
AudioManager.LoadFile(@"Content\xm_title.mp3");

AudioFile xmBoot = AudioManager.GetFileWithName("xm_boot");
AudioFile xmBootOgg = AudioManager.GetFileWithName("xm_boot_ogg");
AudioFile xmTitle = AudioManager.GetFileWithName("xm_title");

xmBoot.Play();
xmBootOgg.Play();

xmTitle.SetVolume(1);
xmTitle.Repeat = -1;
xmTitle.PositionalSound = true;
xmTitle.Play();

Camera camera = new Camera(CameraType.Chase);

cWindow.SetCurrentCamera(camera);

Button btn1 = new Button()
{
    Position = new(150, 150),
    Size = new(44, 44),
    Text = "button",
    BackgroundColor = Color.PaleGoldenrod,
    HoverColor = Color.Goldenrod,
    PressedColor = Color.DarkGoldenrod,
    ForegroundColor = Color.Black,
    Filled = true,
    Font = "Arial.11pt"
};

ListBox listBox1 = new ListBox
{
    Position = new(70, 150),
    Size = new(90, 44),
    BackgroundColor = Color.PaleGoldenrod,
    HoverColor = Color.Goldenrod,
    PressedColor = Color.DarkGoldenrod,
    ForegroundColor = Color.Black,
    Filled = true,
    Font = "Arial.11pt"
};

TextBox textBox1 = new TextBox(300)
{
    Size = new(90, 44),
    Position = new(350, 150),
    BackgroundColor = Color.Red,
    HoverColor = Color.DarkRed,
    PressedColor = Color.Maroon,
    ForegroundColor = Color.White,
    Font = "Arial.11pt",
    Filled = true,
    AllowMultiline = true,
};

CheckBox checkBox1 = new CheckBox
{
    Position = new(500, 150),
    Size = new(40, 40),
    BackgroundColor = Color.BlueViolet,
    HoverColor = Color.Blue,
    PressedColor = Color.LightBlue,
    ForegroundColor = Color.White,
    Font = "Arial.11pt",
    Filled = true
};

listBox1.AddItem(new("test 1"));
listBox1.AddItem(new("test 2"));
listBox1.AddItem(new("dfsdfsdfsdfsdfsdf"));
listBox1.AddItem(new("zxczxzxzx"));
listBox1.AddItem(new("qasqsdfwqer"));

UIManager.AddAsset(cWindow, btn1);
UIManager.AddAsset(cWindow, listBox1);
UIManager.AddAsset(cWindow, textBox1);
UIManager.AddAsset(cWindow, checkBox1);

TextureManager.AddAsset(cWindow, texture);
TextureManager.AddAsset(cWindow, animatedTexture1);

while (cWindow.Run())
{
    SDL.SDL_Event curEvent = cWindow.LastEvent;

    if (cWindow.EventWaiting)
    {
        switch (curEvent.type)
        {
            case SDL.SDL_EventType.SDL_KEYDOWN: // Key is held down.
                Key key = (Key)curEvent.key;

                string keyString = key.ToString();

                switch (keyString)
                {
                    case "LEFT":
                    case "A":
                        camera.Position -= new Vector2(10, 0);
                        break;
                    case "RIGHT":
                    case "D":
                        camera.Position += new Vector2(10, 0);
                        break;
                    case "UP":
                    case "W":
                        camera.Position -= new Vector2(0, 10);
                        break;
                    case "DOWN":
                    case "S":
                        camera.Position += new Vector2(0, 10);
                        break;
                }
                break;
        }
    }

    PrimitiveRenderer.DrawLine(cWindow, new Vector2(500, 300), new Vector2(600, 300), 1, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(cWindow, new Vector2(500, 270), new Vector2(600, 270), 3, Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawLine(cWindow, new Vector2(500, 240), new Vector2(600, 240), 7, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(cWindow, new Vector2(500, 210), new Vector2(600, 210), 15, Color.FromArgb(255, 255, 255, 255), true);

    PrimitiveRenderer.DrawCircle(cWindow, new Vector2(500, 10), new Vector2(50, 50), Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawCircle(cWindow, new Vector2(500, 309), new Vector2(50, 50), Color.FromArgb(127, 255, 255, 255), false);

    PrimitiveRenderer.DrawRectangle(cWindow, new Vector2(552, 10), new Vector2(30, 30), Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawRectangle(cWindow, new Vector2(584, 10), new Vector2(30, 30), Color.FromArgb(33, 0, 0, 255), true);

    PrimitiveRenderer.DrawRoundedRectangle(cWindow, new Vector2(616, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, false);
    PrimitiveRenderer.DrawRoundedRectangle(cWindow, new Vector2(648, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, true);
    PrimitiveRenderer.DrawRoundedRectangle(cWindow, new Vector2(680, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 12, true);

    PrimitiveRenderer.DrawTriangle(cWindow, new Vector2(722, 10), new Vector2(747, 40), new Vector2(707, 40), Color.FromArgb(127, 255, 255, 255), false);
    PrimitiveRenderer.DrawTriangle(cWindow, new Vector2(779, 10), new Vector2(794, 40), new Vector2(764, 40), Color.FromArgb(127, 255, 255, 255), true);

    PrimitiveRenderer.DrawText(cWindow, "#[STRING_TEST]", new Vector2(500, 90), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(cWindow, "Loc string test: #[STRING_TEST]", new Vector2(500, 120), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(cWindow, "Loc string test: #[STRING_TEST] aaaaaa #[STRING_TEST] #[STRING_TEST] bbbbbb", new Vector2(500, 150), Color.FromArgb(255, 0, 0, 255));

    FontManager.DrawText(cWindow, "Test1", "Arial.11pt", new Vector2(700, 10), Color.FromArgb(255, 255, 255, 255));
    FontManager.DrawText(cWindow, "Test2", "Arial.11pt", new Vector2(700, 30), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold);
    FontManager.DrawText(cWindow, "Test3", "Arial.11pt", new Vector2(700, 50), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Italic);
    FontManager.DrawText(cWindow, "Test4", "Arial.11pt", new Vector2(700, 70), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Underline);
    FontManager.DrawText(cWindow, "Test5", "Arial.11pt", new Vector2(700, 90), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Strikeout, 1);
    FontManager.DrawText(cWindow, "Test6", "Arial.11pt", new Vector2(700, 110), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, 3);
    FontManager.DrawText(cWindow, "Test7", "Arial.11pt", new Vector2(700, 130), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, 15, -1);
    FontManager.DrawText(cWindow, "Test8", "Arial.11pt", new Vector2(700, 150), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1);
    FontManager.DrawText(cWindow, "Test9", "Arial.11pt", new Vector2(700, 170), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Shaded);
    FontManager.DrawText(cWindow, "Test10", "Arial.11pt", new Vector2(700, 190), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Solid);
    FontManager.DrawText(cWindow, "Test11", "Arial.11pt", new Vector2(700, 210), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
    FontManager.DrawText(cWindow, "#[STRING_TEST]\nTest2\nTest3", "Arial.11pt", new Vector2(700, 230), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline | SDL_ttf.TTF_FontStyle.Strikeout);

    textureAtlas1.Index = 5;
    textureAtlas1.Position = new Vector2(264, 0);
    textureAtlas1.Draw(cWindow);
    textureAtlas1.Index = 1;
    textureAtlas1.Position = new Vector2(200, 0);
    textureAtlas1.Draw(cWindow);

    for (int x = 0; x < texture.Size.X; x++)
    {
        // textureAPI test
        b += (byte)rnd.Next(-1, 1);
        g += (byte)rnd.Next(-1, 1);
        b += (byte)rnd.Next(-1, 1);
        a += (byte)rnd.Next(-1, 1);

        for (int y = 0; y < texture.Size.Y; y++) texture.SetPixel(x, y, Color.FromArgb(a, r, g, b));
    }

    cWindow.Render();
}