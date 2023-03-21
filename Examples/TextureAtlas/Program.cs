using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics; // for vector2

//Texture atlas example 
// © 2022-2023 starfrost, March 21, 2023

// Initialise Lightning
InitClient();

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings()); // use default Renderersettings

Random Random = new Random();
// create a texture atlas
TextureAtlas texture = new(renderer, new(64, 64), new(4, 4))
{
    Path = "Content/TextureAtlasTest.png",
    Position = new Vector2(200, 200),
};

TextureManager.AddAsset(renderer, texture);

while (renderer.Run())
{
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 200);
    texture.Draw(renderer);
    texture.Index = Random.Next(0, 15);
    texture.Position = new(200, 264);
    texture.Draw(renderer);

    PrimitiveRenderer.DrawText(renderer, "Texture atlas example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}