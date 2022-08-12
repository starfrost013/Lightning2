using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Basic Lightning Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// Load a texture from a file using Texture::Load
Texture texture = new Texture(window, 36, 21) // yes thats the image size blame my lazy cropping
{
    Path = "Content/ParticleEffect.png",
};


texture.Load(window);

// Create a particle effect.
ParticleEffect pe = new ParticleEffect(texture)
{
    MaxNumberCreatedEachFrame = 20,
    Amount = 400,
    Lifetime = 60,
    Variance = 10,
    Position = new(100, 300),
    Velocity = new(3, 3),
    Mode = ParticleMode.AbsoluteVelocity,
};

ParticleManager.AddEffect(window, pe);

while (window.Run())
{
    PrimitiveRenderer.DrawText(window, "Basic example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}