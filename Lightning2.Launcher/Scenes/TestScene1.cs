using LightningGL;
using LightningBase;
using System.Drawing;
using System.Numerics;
using static LightningGL.Lightning;
using System.Diagnostics;

namespace LightningGL
{
    public class TestScene1 : Scene
    {
        /// <summary>
        /// Lazy and stupid. yes, but this is test code
        /// </summary>
        public TextureAtlas? TextureAtlas1 { get; set; }

        public Texture? Texture1 { get; set; }

        public override void Start()
        {
            FontManager.AddAsset(new Font("Arial", 11, "Arial.11pt"));
            FontManager.AddAsset(new Font("Arial", 18, "Arial.18pt"));
            FontManager.AddAsset(new Font("Arial", 24, "Arial.24pt"));
            FontManager.AddAsset(new Font("Arial", 36, "Arial.36pt"));

            Lightning.Renderer.Clear(Color.FromArgb(255, 127, 127, 127));
            Texture1 = new("Texture1", 64, 64);

            // Texture API test
            byte r = (byte)Random.Shared.Next(0, 256);
            byte g = (byte)Random.Shared.Next(0, 256);
            byte b = (byte)Random.Shared.Next(0, 256);
            byte a = (byte)Random.Shared.Next(0, 256);

            for (int x = 0; x < Texture1.Size.X; x++)
            {
                r += (byte)Random.Shared.Next(-5, 5);
                g += (byte)Random.Shared.Next(-5, 5);
                b += (byte)Random.Shared.Next(-5, 5);
                a += (byte)Random.Shared.Next(-5, 5);

                for (int y = 0; y < Texture1.Size.Y; y++) Texture1.SetPixel(x, y, Color.FromArgb(a, r, g, b));
            }

            Texture1.Unlock();

            Texture1.Position = new(0, 0);
            Texture1.Repeat = new(3, 3);

            TextureAtlas1 = new("TextureAtlas1", new(64, 64), new(4, 4))
            {
                Path = @"Content\TextureAtlasTest.png",
                Position = new(256, 256)
            };

            TextureManager.AddAsset(TextureAtlas1);

            AnimatedTexture animatedTexture1 = new AnimatedTexture("AnimatedTexture1", 256, 256, new(0, 3, 1000));
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");

            animatedTexture1.Position = new(320, 256);

            TextureManager.AddAsset(animatedTexture1);

            LightManager.SetEnvironmentalLight(Color.FromArgb(0, 0, 0, 0));

            // todo: particleeffectsettings?

            Texture testEffectTexture = new("testEffectTexture", 16, 16);

            testEffectTexture.Path = @"Content\Sparkles.png";

            ParticleEffect testEffect = new("testEffect", testEffectTexture)
            {
                Amount = 100,
                Lifetime = 70,
                Variance = 40,
                Velocity = new(0.04f, -0.04f),
                Position = new(150, 500),
                MaxNumberCreatedEachFrame = 1,
                Mode = ParticleMode.AbsoluteVelocity,
            };

            ParticleManager.AddAsset(testEffect);

            LightManager.AddAsset(new("Light1")
            {
                Position = new(50, 375),
                Range = 4,
                Brightness = 15,
            });

            LightManager.AddAsset(new("Light2")
            {
                Position = new(250, 300),
                Range = 4,
                Brightness = 31,
            });

            LightManager.AddAsset(new("Light3")
            {
                Position = new(450, 225),
                Range = 4,
                Brightness = 63,
            });

            LightManager.AddAsset(new("Light4")
            {
                Position = new(650, 150),
                Range = 4,
                Brightness = 127,
            });

            LightManager.AddAsset(new("Light5")
            {
                Position = new(850, 75),
                Range = 4,
                Brightness = 255,
            });

            LightManager.AddAsset(new("Light6")
            {
                Position = new(850, 275),
                Range = 4,
                Brightness = 255,
            });

            LightManager.AddAsset(new("Light7")
            {
                Position = new(850, 475),
                Range = 4,
                Brightness = 255,
            });

            LightManager.AddAsset(new("Light8")
            {
                Position = new(850, 675),
                Range = 4,
                Brightness = 255,
            });

            LightManager.AddAsset(new("Light9")
            {
                Position = new(0, 0),
                Range = 4,
                Brightness = 255,
            });

            LightManager.AddAsset(new("Light10")
            {
                Position = new(200, 0),
                LightColor = Color.FromArgb(255, 255, 217, 0),
                Range = 4,
                Brightness = 200,
            });

            AudioManager.AddAsset(new("xm_boot", @"Content\xm_boot.mp3"));
            AudioManager.AddAsset(new("xm_boot_ogg", @"Content\xm_boot_ogg.ogg"));
            AudioManager.AddAsset(new("xm_title", @"Content\xm_title.mp3"));

            AudioFile? xmBoot = (AudioFile?)Lightning.Renderer.GetRenderableByName("xm_boot");
            AudioFile? xmBootOgg = (AudioFile?)Lightning.Renderer.GetRenderableByName("xm_boot_ogg");
            AudioFile? xmTitle = (AudioFile?)Lightning.Renderer.GetRenderableByName("xm_title");

            Debug.Assert(xmBoot != null);
            Debug.Assert(xmBootOgg != null);
            Debug.Assert(xmTitle != null);

            xmBoot.Play();
            xmBootOgg.Play();

            xmTitle.SetVolume(1);
            xmTitle.Repeat = -1;
            xmTitle.PositionalSound = true;
            xmTitle.Play();

            Camera camera = new Camera(CameraType.Chase);

            //camera.ShakeAmount = new(2, 2);
            //camera.Velocity = new(0.2f, 0.2f);
            Lightning.Renderer.SetCurrentCamera(camera);

            LocalSettings.AddSection("Demonstration2");

            Button btn1 = new("Button1", "Arial.11pt")
            {
                Position = new(150, 150),
                Size = new(44, 44),
                Text = "button",
                BackgroundColor = Color.PaleGoldenrod,
                HoverColor = Color.Goldenrod,
                PressedColor = Color.DarkGoldenrod,
                ForegroundColor = Color.Black,
                Filled = true,
            };

            ListBox listBox1 = new("ListBox1", "Arial.11pt")
            {
                Position = new(70, 150),
                Size = new(90, 44),
                BackgroundColor = Color.PaleGoldenrod,
                HoverColor = Color.Goldenrod,
                PressedColor = Color.DarkGoldenrod,
                ForegroundColor = Color.Black,
                Filled = true,
            };

            TextBox textBox1 = new("TextBox1", 300, "Arial.11pt")
            {
                Size = new(90, 44),
                Position = new(350, 150),
                BackgroundColor = Color.Red,
                HoverColor = Color.DarkRed,
                PressedColor = Color.Maroon,
                ForegroundColor = Color.White,
                Filled = true,
                AllowMultiline = true,
            };

            CheckBox checkBox1 = new("CheckBox1", "Arial.11pt")
            {
                Position = new(500, 150),
                Size = new(40, 40),
                BackgroundColor = Color.BlueViolet,
                HoverColor = Color.Blue,
                PressedColor = Color.LightBlue,
                ForegroundColor = Color.White,
                Filled = true
            };


            UIManager.AddAsset(btn1);
            UIManager.AddAsset(listBox1);
            UIManager.AddAsset(textBox1);
            UIManager.AddAsset(checkBox1);

            // you must load a listbox before adding it
            // note this in docs
            listBox1.AddItem(new("Item1", "test 1", "Arial.11pt"));
            listBox1.AddItem(new("Item2", "test 2", "Arial.11pt"));
            listBox1.AddItem(new("Item3", "dfsdfsdfsdfsdfsdf", "Arial.11pt"));
            listBox1.AddItem(new("Item4", "zxczxzxzx", "Arial.11pt"));
            listBox1.AddItem(new("Item5", "qasqsdfwqer", "Arial.11pt"));

            TextureManager.AddAsset(Texture1);

            // bug:
            // as it is the same handle, setpixel changes pixel for every single texture 
            Texture? texture2 = TextureManager.GetInstanceOfTexture(Texture1);

            Debug.Assert(texture2 != null);

            for (int x = 0; x < texture2.Size.X; x++)
            {
                r += (byte)Random.Shared.Next(-5, 5);
                g += (byte)Random.Shared.Next(-5, 5);
                b += (byte)Random.Shared.Next(-5, 5);
                a += (byte)Random.Shared.Next(-5, 5);

                for (int y = 0; y < texture2.Size.Y; y++) texture2.SetPixel(x, y, Color.FromArgb(a, r, g, b));
            }

            texture2.Unlock();
            texture2.Position = new(-200, 0);
            TextureManager.AddAsset(texture2);

            TextureManager.AddAsset(animatedTexture1);

            Animation? anim1 = new Animation("anim1", @"Content\Animations\TestAnimation.json");
            anim1 = AnimationManager.AddAsset(anim1);

            Debug.Assert(anim1 != null);

            Texture1.ZIndex = -9999999;
            Texture1.SetAnimation(anim1);

            Texture1.StartCurrentAnimation();

            PrimitiveManager.AddLine(new(500, 300), new(600, 300), 1, Color.FromArgb(255, 255, 255, 255), false);
            PrimitiveManager.AddLine(new(500, 270), new(600, 270), 3, Color.FromArgb(255, 255, 255, 255), true);
            PrimitiveManager.AddLine(new(500, 240), new(600, 240), 7, Color.FromArgb(255, 255, 255, 255), false);
            PrimitiveManager.AddLine(new(500, 210), new(600, 210), 15, Color.FromArgb(255, 255, 255, 255), true);

            PrimitiveManager.AddCircle(new(500, 10), new(50, 50), Color.FromArgb(255, 255, 255, 255), true);
            PrimitiveManager.AddCircle(new(500, 309), new(50, 50), Color.FromArgb(127, 255, 255, 255), false);

            PrimitiveManager.AddRectangle(new(552, 10), new(30, 30), Color.FromArgb(255, 255, 255, 255), false);
            PrimitiveManager.AddRectangle(new(584, 10), new(30, 30), Color.FromArgb(33, 0, 0, 255), true);

            PrimitiveManager.AddRoundedRectangle(new(616, 10), new(30, 30), Color.FromArgb(127, 255, 255, 255), 3, false);
            PrimitiveManager.AddRoundedRectangle(new(648, 10), new(30, 30), Color.FromArgb(127, 255, 255, 255), 3, true);
            PrimitiveManager.AddRoundedRectangle(new(680, 10), new(30, 30), Color.FromArgb(127, 255, 255, 255), 12, true);

            PrimitiveManager.AddTriangle(new(722, 10), new(747, 40), new(707, 40), Color.FromArgb(127, 255, 255, 255), false);
            PrimitiveManager.AddTriangle(new(794, 40), new(764, 40), new(779, 10), Color.FromArgb(127, 255, 255, 255), true, false, new(10, 10), Color.Yellow);

            TextManager.DrawText("#[STRING_TEST]", "DebugFont", new(500, 90), Color.FromArgb(255, 0, 0, 255));
            TextManager.DrawText("Loc string test: #[STRING_TEST]", "DebugFont", new(500, 120), Color.FromArgb(255, 0, 0, 255));
            TextManager.DrawText("Loc string test: #[STRING_TEST] aaaaaa #[STRING_TEST] #[STRING_TEST] bbbbbb", "DebugFont", new(500, 150), Color.FromArgb(255, 0, 0, 255));

            TextManager.DrawText("Test1", "Arial.11pt", new(700, 10), Color.FromArgb(255, 255, 255, 255));
            TextManager.DrawText("Test2", "Arial.11pt", new(700, 30), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold);
            TextManager.DrawText("Test3", "Arial.11pt", new(700, 50), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Italic);
            TextManager.DrawText("Test4", "Arial.11pt", new(700, 70), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Underline);
            TextManager.DrawText("Test5", "Arial.11pt", new(700, 90), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Strikeout, 1);
            TextManager.DrawText("Test6", "Arial.11pt", new(700, 110), Color.FromArgb(255, 255, 255, 0), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, 3);
            TextManager.DrawText("Test7", "Arial.11pt", new(700, 130), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, 15, -1);
            TextManager.DrawText("Test8", "Arial.11pt", new(700, 150), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1);
            TextManager.DrawText("Test9", "Arial.11pt", new(700, 170), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Shaded);
            TextManager.DrawText("Test10", "Arial.11pt", new(700, 190), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold, -1, -1, FontSmoothingType.Solid);
            TextManager.DrawText("Test11", "Arial.11pt", new(700, 210), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
            TextManager.DrawText("#[STRING_TEST]\nTest2\nTest3", "Arial.11pt", new(700, 230), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline | SDL_ttf.TTF_FontStyle.Strikeout);

        }

        public override void Shutdown()
        {

        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void SwitchTo(Scene oldScene)
        {

        }

        public override void Render()
        {
            // TODO: hack until the old event system is completely deprecated
            SdlRenderer sdlRenderer = (SdlRenderer)Lightning.Renderer;

            SDL.SDL_Event curEvent = sdlRenderer.LastEvent;

            Camera camera = Lightning.Renderer.Settings.Camera;

            if (Lightning.Renderer.EventWaiting)
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

           
            if (TextureAtlas1 == null) throw new NullReferenceException();

            TextureAtlas1.Index = 5;
            TextureAtlas1.Position = new(264, 0);
            TextureAtlas1.DrawFrame();
            TextureAtlas1.Index = 1;
            TextureAtlas1.Position = new(200, 0);
            TextureAtlas1.DrawFrame();

            // Texture API test
            byte r = (byte)Random.Shared.Next(0, 256);
            byte g = (byte)Random.Shared.Next(0, 256);
            byte b = (byte)Random.Shared.Next(0, 256);
            byte a = (byte)Random.Shared.Next(0, 256);

            if (Texture1 == null) throw new NullReferenceException();

            for (int x = 0; x < Texture1.Size.X; x++)
            {
                // textureAPI test
                r += (byte)Random.Shared.Next(-1, 1);
                g += (byte)Random.Shared.Next(-1, 1);
                b += (byte)Random.Shared.Next(-1, 1);
                a += (byte)Random.Shared.Next(-1, 1);

                for (int y = 0; y < Texture1.Size.Y; y++) Texture1.SetPixel(x, y, Color.FromArgb(a, r, g, b));
            }
        }
    }
}
