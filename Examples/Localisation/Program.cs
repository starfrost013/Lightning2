using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Localisation example
// © 2022-2023 starfrost, March 21, 2023

// Initialise Lightning
Lightning.InitClient();

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

while (Renderer.Run())
{
    // #[name] is used for localisation strings
    PrimitiveRenderer.DrawText(Renderer, "#[LOC_STRING_01]", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(Renderer, "#[LOC_STRING_02]", new Vector2(100, 150), Color.Red); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(Renderer, "#[LOC_STRING_03]", new Vector2(100, 200), Color.RebeccaPurple); // no fonts loaded so we use the debug font
    Renderer.Render();
}