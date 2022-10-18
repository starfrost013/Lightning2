using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture animation example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Init(args);

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings()); // use default Renderersettings

AnimatedTexture texture = new AnimatedTexture(renderer,
    256,
    256,
    new AnimationCycle(0, 3, 60));

// add frames to the texture
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF0.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF1.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF2.png");
texture.AddFrame(@"Content\AnimTextureTest\AnimTextureTestF3.png");
texture.Position = new(200, 200);
TextureManager.AddAsset(renderer, texture);

while (renderer.Run())
{
    PrimitiveRenderer.DrawText(renderer, "Animated texture example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}