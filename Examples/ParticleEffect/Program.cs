using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Particle effects example
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

// Load a texture from a file using Texture::Load
Texture texture = new Texture(Renderer, 18, 11) // yes thats the image size blame my lazy cropping
{
    Path = "Content/ParticleEffect.png",
};


texture.Load(Renderer);

// Create a particle effect.
ParticleEffect particleEffect = new ParticleEffect(texture)
{
    MaxNumberCreatedEachFrame = 3,
    FrameSkipBetweenCreatingParticles = 2,
    Amount = 200,
    Lifetime = 200,
    Variance = 10,
    Position = new(100, 300),
    Velocity = new(4f, 4f), // it's a float so use f
    Mode = ParticleMode.Explode,
};

ParticleManager.AddEffect(Renderer, particleEffect);

while (Renderer.Run())
{
    PrimitiveRenderer.DrawText(Renderer, "Particle effect example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    Renderer.Render();
}