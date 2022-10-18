using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture atlas example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

Random Random = new Random();
// create a texture atlas
TextureAtlas texture = new TextureAtlas(Renderer, new(64, 64), new(4, 4))
{
    Path = "Content/TextureAtlasTest.png",
    Position = new Vector2(200, 200),
};

texture.Load(Renderer);

while (Renderer.Run())
{
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 200);
    texture.Draw(Renderer);
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 264);
    texture.Draw(Renderer);

    PrimitiveRenderer.DrawText(Renderer, "Texture atlas example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    Renderer.Render();
}