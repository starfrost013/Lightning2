using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture animation example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

Random Random = new Random();

AnimatedTexture texture = new AnimatedTexture(256, 256)
{
    Cycle = new AnimationCycle(0, 3, 60)
};

// add frames to the texture
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");
texture.Position = new(200, 200);
texture.Load(Renderer);

while (Renderer.Run())
{
    texture.Draw(Renderer);

    PrimitiveRenderer.DrawText(Renderer, "Animated texture example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    Renderer.Render();
}