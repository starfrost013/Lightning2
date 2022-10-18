// See https://aka.ms/new-console-template for more information
// Test LightningGL application

// © 2022 starfrost

using LightningGL;
using LightningBase;
using System.Drawing;
using System.Numerics;
using static LightningGL.Lightning;

Init(args); // Initialise LightningGL
Renderer cRenderer = new Renderer();
RendererSettings windowSettings = new RendererSettings();

windowSettings.Title = "LightningGL Example";

cRenderer.Start(windowSettings);

FontManager.LoadFont("Arial", 11, "Arial.11pt");
FontManager.LoadFont("Arial", 18, "Arial.18pt");
FontManager.LoadFont("Arial", 24, "Arial.24pt");
FontManager.LoadFont("Arial", 36, "Arial.36pt");

cRenderer.Clear(Color.FromArgb(255, 127, 127, 127));
Texture texture = new(cRenderer, 64, 64);

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

texture.Position = new(0, 0);
texture.Repeat = new(3, 3);

TextureAtlas textureAtlas1 = new(cRenderer, new(64, 64), new(4, 4));

textureAtlas1.Path = @"Content\TextureAtlasTest.png";

textureAtlas1.Position = new(256, 256);

TextureManager.AddAsset(cRenderer, textureAtlas1);

AnimatedTexture animatedTexture1 = new AnimatedTexture(cRenderer, 256, 256, new(0, 3, 1000));
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");

animatedTexture1.Position = new(320, 256);

TextureManager.AddAsset(cRenderer, animatedTexture1);

LightManager.SetEnvironmentalLight(Color.FromArgb(0, 0, 0, 0));

// todo: particleeffectsettings?

Texture testEffectTexture = new(cRenderer, 16, 16);

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

ParticleManager.AddAsset(cRenderer, testEffect);

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(50, 375),
    Range = 4,
    Brightness = 15,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(250, 300),
    Range = 4,
    Brightness = 31,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(450, 225),
    Range = 4,
    Brightness = 63,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(650, 150),
    Range = 4,
    Brightness = 127,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(850, 75),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(850, 275),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(850, 475),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(850, 675),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(0, 0),
    Range = 4,
    Brightness = 255,
});

LightManager.AddAsset(cRenderer, new Light
{
    Position = new(200, 0),
    LightColor = Color.FromArgb(255, 255, 217, 0),
    Range = 4,
    Brightness = 200,
}) ;

AudioManager.LoadFile(cRenderer, @"Content\xm_boot.mp3");
AudioManager.LoadFile(cRenderer, @"Content\xm_boot_ogg.ogg");
AudioManager.LoadFile(cRenderer, @"Content\xm_title.mp3");

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

//camera.CameraShakeAmount = new(2, 2);
//camera.Velocity = new(0.2f, 0.2f);
cRenderer.SetCurrentCamera(camera);

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

UIManager.AddAsset(cRenderer, btn1);
UIManager.AddAsset(cRenderer, listBox1);
UIManager.AddAsset(cRenderer, textBox1);
UIManager.AddAsset(cRenderer, checkBox1);

TextureManager.AddAsset(cRenderer, texture);

// bug:
// as it is the same handle, setpixel changes pixel for every single texture 
Texture texture2 = TextureManager.GetInstanceOfTexture(cRenderer, texture);

for (int x = 0; x < texture2.Size.X; x++)
{
    r += (byte)rnd.Next(-5, 5);
    g += (byte)rnd.Next(-5, 5);
    b += (byte)rnd.Next(-5, 5);
    a += (byte)rnd.Next(-5, 5);

    for (int y = 0; y < texture2.Size.Y; y++) texture2.SetPixel(x, y, Color.FromArgb(a, r, g, b));
}

texture2.Unlock();
texture2.Position = new(-200, 0);
TextureManager.AddAsset(cRenderer, texture2);

TextureManager.AddAsset(cRenderer, animatedTexture1);

Animation anim1 = new Animation(@"Content\Animations\TestAnimation.json");
anim1 = AnimationManager.AddAsset(cRenderer, anim1);

texture.ZIndex = -9999999;
texture.SetAnimation(anim1);

anim1.StartAnimationFor(texture);
while (cRenderer.Run())
{
    SDL.SDL_Event curEvent = cRenderer.LastEvent;

    if (cRenderer.EventWaiting)
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
    
    PrimitiveRenderer.DrawLine(cRenderer, new Vector2(500, 300), new Vector2(600, 300), 1, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(cRenderer, new Vector2(500, 270), new Vector2(600, 270), 3, Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawLine(cRenderer, new Vector2(500, 240), new Vector2(600, 240), 7, Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(cRenderer, new Vector2(500, 210), new Vector2(600, 210), 15, Color.FromArgb(255, 255, 255, 255), true);

    PrimitiveRenderer.DrawCircle(cRenderer, new Vector2(500, 10), new Vector2(50, 50), Color.FromArgb(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawCircle(cRenderer, new Vector2(500, 309), new Vector2(50, 50), Color.FromArgb(127, 255, 255, 255), false);

    PrimitiveRenderer.DrawRectangle(cRenderer, new Vector2(552, 10), new Vector2(30, 30), Color.FromArgb(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawRectangle(cRenderer, new Vector2(584, 10), new Vector2(30, 30), Color.FromArgb(33, 0, 0, 255), true);

    PrimitiveRenderer.DrawRoundedRectangle(cRenderer, new Vector2(616, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, false);
    PrimitiveRenderer.DrawRoundedRectangle(cRenderer, new Vector2(648, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 3, true);
    PrimitiveRenderer.DrawRoundedRectangle(cRenderer, new Vector2(680, 10), new Vector2(30, 30), Color.FromArgb(127, 255, 255, 255), 12, true);

    PrimitiveRenderer.DrawTriangle(cRenderer, new Vector2(722, 10), new Vector2(747, 40), new Vector2(707, 40), Color.FromArgb(127, 255, 255, 255), false);
    PrimitiveRenderer.DrawTriangle(cRenderer, new Vector2(779, 10), new Vector2(794, 40), new Vector2(764, 40), Color.FromArgb(127, 255, 255, 255), true);

    PrimitiveRenderer.DrawText(cRenderer, "#[STRING_TEST]", new Vector2(500, 90), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(cRenderer, "Loc string test: #[STRING_TEST]", new Vector2(500, 120), Color.FromArgb(255, 0, 0, 255));
    PrimitiveRenderer.DrawText(cRenderer, "Loc string test: #[STRING_TEST] aaaaaa #[STRING_TEST] #[STRING_TEST] bbbbbb", new Vector2(500, 150), Color.FromArgb(255, 0, 0, 255));

    TextManager.DrawText(cRenderer, "Test1", "Arial.11pt", new Vector2(700, 10), Color.FromArgb(255, 255, 255, 255));
    TextManager.DrawText(cRenderer, "Test2", "Arial.11pt", new Vector2(700, 30), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold);
    TextManager.DrawText(cRenderer, "Test3", "Arial.11pt", new Vector2(700, 50), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Italic);
    TextManager.DrawText(cRenderer, "Test4", "Arial.11pt", new Vector2(700, 70), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Underline);
    TextManager.DrawText(cRenderer, "Test5", "Arial.11pt", new Vector2(700, 90), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Strikeout, 1);
    TextManager.DrawText(cRenderer, "Test6", "Arial.11pt", new Vector2(700, 110), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, 3);
    TextManager.DrawText(cRenderer, "Test7", "Arial.11pt", new Vector2(700, 130), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, 15, -1);
    TextManager.DrawText(cRenderer, "Test8", "Arial.11pt", new Vector2(700, 150), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1);
    TextManager.DrawText(cRenderer, "Test9", "Arial.11pt", new Vector2(700, 170), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Shaded);
    TextManager.DrawText(cRenderer, "Test10", "Arial.11pt", new Vector2(700, 190), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Solid);
    TextManager.DrawText(cRenderer, "Test11", "Arial.11pt", new Vector2(700, 210), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
    TextManager.DrawText(cRenderer, "#[STRING_TEST]\nTest2\nTest3", "Arial.11pt", new Vector2(700, 230), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline | SDL_ttf.TTF_FontStyle.Strikeout);

    textureAtlas1.Index = 5;
    textureAtlas1.Position = new Vector2(264, 0);
    textureAtlas1.Draw(cRenderer);
    textureAtlas1.Index = 1;
    textureAtlas1.Position = new Vector2(200, 0);
    textureAtlas1.Draw(cRenderer);

    for (int x = 0; x < texture.Size.X; x++)
    {
        // textureAPI test
        b += (byte)rnd.Next(-1, 1);
        g += (byte)rnd.Next(-1, 1);
        b += (byte)rnd.Next(-1, 1);
        a += (byte)rnd.Next(-1, 1);

        for (int y = 0; y < texture.Size.Y; y++) texture.SetPixel(x, y, Color.FromArgb(a, r, g, b));
    }

    cRenderer.Render();
}