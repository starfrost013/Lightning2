using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Lightning lighting Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// set the environmental light
LightManager.SetEnvironmentalLight(Color.Black);

Light newLight = new Light
{
    Brightness = 100,
    Range = 20,
    Position = new Vector2(100, 100)
};

while (window.Run())
{
    PrimitiveRenderer.DrawText(window, "Lighting example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}