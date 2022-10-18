using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Basic Lightning Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022 (Lightning 1.1 version - October 18, 2022)

// Initialise Lightning
Lightning.Init(args);

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings
{
    Title = "Basic Lightning2 Demo"
}); // use default windowsettings except title

while (renderer.Run())
{
    PrimitiveRenderer.DrawText(renderer, "Basic example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}