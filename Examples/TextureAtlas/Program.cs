using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture atlas example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

Random Random = new Random();
// create a texture atlas
TextureAtlas texture = new TextureAtlas(window, new(64, 64), new(4, 4))
{
    Path = "Content/TextureAtlasTest.png",
    Position = new Vector2(200, 200),
};

texture.Load(window);

while (window.Run())
{
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 200);
    texture.Draw(window);
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 250);
    texture.Draw(window);

    PrimitiveRenderer.DrawText(window, "Texture atlas example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}