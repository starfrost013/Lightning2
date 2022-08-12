using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Basic Lightning Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// Load a texture
Texture texture = new Texture();

// Create a particle effect.
ParticleEffect pe = new ParticleEffect()
{
    MaxNumberCreatedEachFrame = 20,

};

{
    PrimitiveRenderer.DrawText(window, "Basic example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}