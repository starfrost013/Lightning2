using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Lightning Primitive Rendering Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

while (window.Run())
{
    PrimitiveRenderer.DrawPixel(window, new Vector2(20, 100), Color.FromArgb(255, 100, 100, 255)); // draw a pixel @ 0,0, colour ARGB 255,100,100,255
    PrimitiveRenderer.DrawLine(window, new Vector2(40, 100), new Vector2(40, 150), 5, Color.Blue); // draw a line from 10,0 to 50,0, thickness 5, colour blue
    PrimitiveRenderer.DrawCircle(window, new Vector2(40, 100), new Vector2(25, 25), Color.Yellow, false, true); // draw an anti-aliased, yellow unfilled circle size (25,25) at (40,100)
    PrimitiveRenderer.DrawCircle(window, new Vector2(80, 100), new Vector2(25, 25), Color.Yellow, true); // draw a yellow filled circle size (25,25) at (80,100)
    PrimitiveRenderer.DrawRectangle(window, new Vector2(120, 100), new Vector2(25, 25), Color.Orange, false); // draw an orange unfilled rectangle size (25,25) at (120,100)
    PrimitiveRenderer.DrawRectangle(window, new Vector2(150, 100), new Vector2(25, 25), Color.OrangeRed, true); // draw an orange-red filled rectangle size (25,25) at (150,100)
    PrimitiveRenderer.DrawRoundedRectangle(window, new Vector2(180, 100), new Vector2(25, 25), Color.Red, 5, false); // draw a red unfilled rounded rectangle size (25,25) at (180,100)


    PrimitiveRenderer.DrawText(window, "Basic example", new Vector2(560, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}