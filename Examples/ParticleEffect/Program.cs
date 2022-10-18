using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics;

//Particle effects example
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Init(args);

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings()); // use default Renderersettings

// Load a texture from a file using Texture::Load
Texture texture = new Texture(renderer, 18, 11) // yes thats the image size blame my lazy cropping
{
    Path = "Content/ParticleEffect.png",
};


TextureManager.AddAsset(renderer, texture);

// Create a particle effect.
ParticleEffect particleEffect = new ParticleEffect(texture)
{
    MaxNumberCreatedEachFrame = 3,
    FrameSkipBetweenCreatingParticles = 2,
    Amount = 200,
    Lifetime = 5000,
    Variance = 25,
    Position = new(100, 300),
    Velocity = new(0.1f, 0.1f), // it's a float so use f
    Mode = ParticleMode.Explode,
};

ParticleManager.AddAsset(renderer, particleEffect);

while (renderer.Run())
{
    PrimitiveRenderer.DrawText(renderer, "Particle effect example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}