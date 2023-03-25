using LightningGL;
using LightningBase;
using System.Drawing;
using System.Numerics;
using static LightningGL.Lightning;
using System.Diagnostics;
using LightningUtil;

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
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", 11, "Arial.11pt"));
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", 18, "Arial.18pt"));
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", 24, "Arial.24pt"));
            Lightning.Renderer.AddRenderable(new Font("Arial.ttf", 36, "Arial.36pt"));

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

            Lightning.Renderer.AddRenderable(TextureAtlas1);

            AnimatedTexture animatedTexture1 = new("AnimatedTexture1", 256, 256, new(0, 3, 1000));
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
            animatedTexture1.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");

            animatedTexture1.Position = new(320, 256);

            Lightning.Renderer.AddRenderable(animatedTexture1);

            LightManager.SetEnvironmentalLight(Color.FromArgb(0, 0, 0, 0));

            // todo: particleeffectsettings?

            Texture testEffectTexture = new("testEffectTexture", 16, 16, false, @"Content\Sparkles.png");
            Texture missingTextureTest = new("DONOTCREATETHISTEXTUREFILE", 128, 128, false, "asdjasdjasjdhasjhsahjasdhjashjasdhsa");

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

            Lightning.Renderer.AddRenderable(testEffect);

            /*
            Lightning.Renderer.AddRenderable(new Light("Light1")
            {
                Position = new(50, 375),
                Range = 4,
                Brightness = 15,
            });

            Lightning.Renderer.AddRenderable(new Light("Light2")
            {
                Position = new(250, 300),
                Range = 4,
                Brightness = 31,
            });

            Lightning.Renderer.AddRenderable(new Light("Light3")
            { 
                Position = new(450, 225), 
                Range = 4, 
                Brightness = 63 });

            Lightning.Renderer.AddRenderable(new Light("Light4")
            { 
                Position = new(650, 150), 
                Range = 4, 
                Brightness = 127 });

            Lightning.Renderer.AddRenderable(new Light("Light5")
            { 
                Position = new(850, 75), 
                Range = 4, 
                Brightness = 255 
            });

            Lightning.Renderer.AddRenderable(new Light("Light6")
            { 
                Position = new(850, 275), 
                Range = 4, 
                Brightness = 255 
            });
            */
            Lightning.Renderer.AddRenderable(new Light("Light7")
            { 
                Position = new(850, 475), 
                Range = 4, 
                Brightness = 127
            });

            Lightning.Renderer.AddRenderable(new Light("Light9")
            {
                Position = new(300, 300),
                Range = 4,
                LightColor = Color.FromArgb(255, 255, 0, 0),
                Brightness = 127
            });

            Lightning.Renderer.AddRenderable(new Light("Light9")
            { 
                Position = new(350, 300), 
                Range = 4,
                LightColor = Color.FromArgb(255, 0, 255, 0),
                Brightness = 127
            });

            Lightning.Renderer.AddRenderable(new Light("Light10")
            {
                Position = new(325, 350),
                LightColor = Color.FromArgb(255, 0, 0, 255),
                Range = 4,
                Brightness = 255,
            });

            Lightning.Renderer.AddRenderable(new Audio("xm_boot", @"Content\xm_boot.mp3"));
            Lightning.Renderer.AddRenderable(new Audio("xm_boot_ogg", @"Content\xm_boot_ogg.ogg"));
            Lightning.Renderer.AddRenderable(new Audio("xm_title", @"Content\xm_title.mp3"));

            Audio? xmBoot = (Audio?)Lightning.Renderer.GetRenderableByName("xm_boot");
            Audio? xmBootOgg = (Audio?)Lightning.Renderer.GetRenderableByName("xm_boot_ogg");
            Audio? xmTitle = (Audio?)Lightning.Renderer.GetRenderableByName("xm_title");

            Debug.Assert(xmBoot != null);
            Debug.Assert(xmBootOgg != null);
            Debug.Assert(xmTitle != null);

            xmBoot.Play();
            xmBootOgg.Play();

            xmTitle.SetVolume(1);
            xmTitle.Repeat = -1;
            xmTitle.PositionalSound = true;
            xmTitle.Play();

            Camera camera = new(CameraType.Chase);

            //camera.ShakeAmount = new(2, 2);
            //camera.Velocity = new(0.2f, 0.2f);
            Lightning.Renderer.SetCurrentCamera(camera);

            LocalSettings.AddSection("Demonstration2");

            Button btn1 = new("Button1", "Arial.11pt")
            {
                Position = new(50, 50),
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


            Lightning.Renderer.AddRenderable(btn1);
            Lightning.Renderer.AddRenderable(listBox1);
            Lightning.Renderer.AddRenderable(textBox1);
            Lightning.Renderer.AddRenderable(checkBox1);

            // you must load a listbox before adding it
            // note this in docs
            listBox1.AddItem(new("Item1", "Arial.11pt", "test 1"));
            listBox1.AddItem(new("Item2", "Arial.11pt", "test 2"));
            listBox1.AddItem(new("Item3", "Arial.11pt", "dfsdfsdfsdfsdfsdf"));
            listBox1.AddItem(new("Item4", "Arial.11pt", "zxczxzxzx"));
            listBox1.AddItem(new("Item5", "Arial.11pt", "qasqsdfwqer"));

            Lightning.Renderer.AddRenderable(Texture1);
            Lightning.Renderer.AddRenderable(missingTextureTest);

            missingTextureTest.Position = new(150, 150);
            // bug:
            // as it is the same handle, setpixel changes pixel for every single texture 
            Texture? texture2 = TextureUtils.CloneTexture(Texture1);

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
            Lightning.Renderer.AddRenderable(texture2);

            Lightning.Renderer.AddRenderable(animatedTexture1);

            Animation? anim1 = new("anim1", @"Content\Animations\Animation1.json");
            anim1 = Lightning.Renderer.AddRenderable(anim1);

            Debug.Assert(anim1 != null);

            Texture1.ZIndex = -9999999;
            Texture1.SetAnimation(anim1);

            Texture1.StartCurrentAnimation();

            Lightning.Renderer.AddRenderable(new Line("Line1", new(500, 300), new(600, 300), Color.FromArgb(255, 255, 255, 255)));
            Lightning.Renderer.AddRenderable(new Line("Line2", new(500, 270), new(600, 270), Color.FromArgb(255, 255, 255, 255)));
            Lightning.Renderer.AddRenderable(new Line("Line3", new(500, 240), new(600, 240), Color.FromArgb(255, 255, 255, 255)));
            Lightning.Renderer.AddRenderable(new Line("Line4", new(500, 210), new(600, 210), Color.FromArgb(255, 255, 255, 255)));

            Lightning.Renderer.AddRenderable(new Ellipse("Ellipse1", new(500, 10), new(50, 50), Color.FromArgb(255, 255, 255, 255)));

            Lightning.Renderer.AddRenderable(new Ellipse("Ellipse2", new(500, 309), new(50, 50), Color.FromArgb(127, 255, 255, 255)));

            Lightning.Renderer.AddRenderable(new Rectangle("Rect1", new(552, 10), new(30, 30), Color.FromArgb(255, 255, 255, 255), false));
            Lightning.Renderer.AddRenderable(new Rectangle("Rect2", new(584, 10), new(30, 30), Color.FromArgb(33, 0, 0, 255), true));

            Lightning.Renderer.AddRenderable(new TextBlock("text1", "#[STRING_TEST]", "DebugFont", new(500, 90), Color.FromArgb(255, 0, 0, 255)));
            Lightning.Renderer.AddRenderable(new TextBlock("text2", "Loc string test: #[STRING_TEST]", "DebugFont", new(500, 120), Color.FromArgb(255, 0, 0, 255)));
            Lightning.Renderer.AddRenderable(new TextBlock("text3", "Loc string test: #[STRING_TEST] aaaaaa #[STRING_TEST] #[STRING_TEST] bbbbbb", "DebugFont", 
                new(500, 150), Color.FromArgb(255, 0, 0, 255)));

            Lightning.Renderer.AddRenderable(new TextBlock("text4", "Test1", "Arial.11pt", new(700, 10), Color.FromArgb(255, 255, 255, 255)));
            Lightning.Renderer.AddRenderable(new TextBlock("text5", "Test2", "Arial.11pt", new(700, 30), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0)));
            Lightning.Renderer.AddRenderable(new TextBlock("text6", "Test3", "Arial.11pt", new(700, 50), Color.FromArgb(255, 255, 255, 0), 
                Color.FromArgb(255, 255, 0, 0)));
            Lightning.Renderer.AddRenderable(new TextBlock("text7", "Test4", "Arial.11pt", new(700, 70), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0)));
            Lightning.Renderer.AddRenderable(new TextBlock("text8", "Test5", "Arial.11pt", new(700, 90), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0), 1));
            Lightning.Renderer.AddRenderable(new TextBlock("text9", "Test6", "Arial.11pt", new(700, 110), Color.FromArgb(255, 255, 255, 0), 
                Color.FromArgb(255, 255, 0, 0), -1));
            Lightning.Renderer.AddRenderable(new TextBlock("text10", "Test7", "Arial.11pt", new(700, 130), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0), 15));
            Lightning.Renderer.AddRenderable(new TextBlock("text11", "Test8", "Arial.11pt", new(700, 150), Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 0, 0), -1));
            Lightning.Renderer.AddRenderable(new TextBlock("text12", "Test9", "Arial.11pt", new(700, 190), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0),  -1));
            Lightning.Renderer.AddRenderable(new TextBlock("text13", "Test10", "Arial.11pt", new(700, 210), Color.FromArgb(255, 255, 255, 255), 
                Color.FromArgb(255, 255, 0, 0)));
            Lightning.Renderer.AddRenderable(new TextBlock("text14", "#[STRING_TEST]\nMulti-line text test\nTest3", "Arial.11pt", new(700, 230), 
                Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 0, 0)));

            Lightning.Renderer.AddRenderable(new TextBlock("text15", "pos_test", "Arial.36pt", new(0, 0), Color.FromArgb(255, 127, 0, 255)));
        }

        public override void Shutdown()
        {
            Logger.Log("Scene Shutdown event works!");
        }

        public override void SwitchFrom(Scene newScene)
        {
            Logger.Log("Scene SwitchFrom event works!");
        }

        public override void SwitchTo(Scene? oldScene)
        {
            Logger.Log("Scene SwitchTo event works!");
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
                    // HIGHLY DEPRECATED METHOD !!!!!!!!!!!!!!!!!!!!!
                    // DO NOT PERFORM THIS ACTION !!!!!!!!!!!!!!!!!!!!!!!
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


            Debug.Assert(TextureAtlas1 != null);

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
