using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics;

//Lightning collision example 
// © 2022-2023 starfrost, March 21, 2023

// Initialise Lightning
InitClient();

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings
{
    Title = "Lightning2 Packaging/WAD Demo"
}); // use default Renderersettings except title

Texture coll1 = new Texture(renderer, 128, 128)
{
    Path = "Content/CollidingTexture1.png",
    Position = new Vector2(128, 300),
};

Texture coll2 = new Texture(renderer, 128, 128)
{
    Path = "Content/CollidingTexture2.png",
    Position = new Vector2(renderer.Settings.Size.X - 128, 300),
};

TextureManager.AddAsset(renderer, coll1);
TextureManager.AddAsset(renderer, coll2);

while (renderer.Run())
{
    if (!AABB.Intersects(coll1, coll2))
    {
        coll1.Position = new Vector2(coll1.Position.X + 0.1f, coll1.Position.Y);
        coll2.Position = new Vector2(coll2.Position.X - 0.1f, coll2.Position.Y);
    }

    PrimitiveRenderer.DrawText(renderer, "Collision example extracted from WAD file", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}