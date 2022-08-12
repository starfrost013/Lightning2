using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture animation example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

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
texture.Load(window);

while (window.Run())
{
    texture.Draw(window);

    PrimitiveRenderer.DrawText(window, "Animated texture example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}