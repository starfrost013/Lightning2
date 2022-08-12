using LightningGL; // use lightninggl
using static NuCore.SDL2.SDL; // for sdl 
using System.Drawing; // for color
using System.Numerics;

//Lightning Primitive Rendering Example
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// setup camera
Camera camera = new Camera(CameraType.Follow);
window.SetCurrentCamera(camera);

while (window.Run())
{
    PrimitiveRenderer.DrawPixel(window, new Vector2(20, 100), Color.FromArgb(255, 100, 100, 255)); // draw a pixel @ 0,0, colour ARGB 255,100,100,255
    PrimitiveRenderer.DrawLine(window, new Vector2(40, 100), new Vector2(40, 250), 5, Color.Blue); // draw a line from 10,0 to 50,0, thickness 5, colour blue
    PrimitiveRenderer.DrawCircle(window, new Vector2(40, 100), new Vector2(25, 25), Color.Yellow, false, true); // draw an anti-aliased, yellow unfilled circle size (25,25) at (40,100)
    PrimitiveRenderer.DrawCircle(window, new Vector2(93, 100), new Vector2(25, 25), Color.Yellow, true); // draw a yellow filled circle size (25,25) at (93,100)
    PrimitiveRenderer.DrawRectangle(window, new Vector2(120, 100), new Vector2(25, 25), Color.Orange, false); // draw an orange unfilled rectangle size (25,25) at (120,100)
    PrimitiveRenderer.DrawRectangle(window, new Vector2(150, 100), new Vector2(25, 25), Color.OrangeRed, true); // draw an orange-red filled rectangle size (25,25) at (150,100)
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(180, 100), new Vector2(25, 25), Color.Red, 5, false); // draw a red unfilled rounded rectangle, corner radius 5px, size (25,25) at (180,100)
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(210, 100), new Vector2(25, 25), Color.Red, 5, true); // draw a red filled rounded rectangle, corner radius 5px, size (25,25) at (210,100)
    PrimitiveRenderer.DrawTriangle(window, new Vector2(255, 100), new Vector2(270, 125), new Vector2(240, 125), Color.Turquoise, false); // draw a turqoise unfilled triangle with points (255,100) (270,125) (240,125) 
    PrimitiveRenderer.DrawTriangle(window, new Vector2(290, 100), new Vector2(305, 125), new Vector2(275, 125), Color.Turquoise, true); // draw a turqoise filled triangle with points (290,100) (305,125) (275,125)

    // Basically a diamond
    List<Vector2> polyPoints1 = new List<Vector2>
    { new Vector2(400, 100),
      new Vector2(430, 130),
      new Vector2(460, 100),
      new Vector2(430, 70),
      new Vector2(400, 100)
    };

    // Also a diamond but offset
    List<Vector2> polyPoints2 = new List<Vector2>
    { new Vector2(400, 200),
      new Vector2(430, 230),
      new Vector2(460, 200),
      new Vector2(430, 170),
      new Vector2(400, 200)
    };

    PrimitiveRenderer.DrawPolygon(window, polyPoints1, Color.BlueViolet, false); // draw an unfilled blue-violet polygon with the points polyPoints1
    PrimitiveRenderer.DrawPolygon(window, polyPoints2, Color.BlueViolet, true); // draw a filled blue-violet polygon with the points polyPoints2

    PrimitiveRenderer.DrawText(window, "Camera example", new Vector2(300, 300), Color.White); // no fonts loaded so we use the debug font

    if (window.EventWaiting)
    {
        switch (window.LastEvent.type)
        {
            case SDL_EventType.SDL_KEYDOWN: // Key is held down.
                Key key = (Key)window.LastEvent.key;

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

    window.Render();
}