using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Lightning lighting Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

// make the Renderer white to emphasise the light
Renderer.Clear(Color.White);
// set the environmental light
LightManager.SetEnvironmentalLight(Color.Black);

Light newLight = new Light
{
    Brightness = 140, // range 0-255, (255 - value) = lowest alpha range in environmental light
    Range = 8,
    Position = new Vector2(150, 125)
};

LightManager.AddLight(Renderer, newLight);

while (Renderer.Run())
{
    PrimitiveRenderer.DrawText(Renderer, "Lighting example", new Vector2(100, 100), Color.Black); // no fonts loaded so we use the debug font
    Renderer.Render();
}