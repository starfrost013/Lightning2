using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Lightning collision example (Scene Manager NOT used)
//©2022 starfrost, August 13, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings
{
    Title = "Basic Lightning2 Demo"
}); // use default Renderersettings except title

Texture coll1 = new Texture(Renderer, 128, 128)
{
    Path = "Content/CollidingTexture1.png",
    Position = new Vector2(128, 300),
};

Texture coll2 = new Texture(Renderer, 128, 128)
{
    Path = "Content/CollidingTexture2.png",
    Position = new Vector2(Renderer.Settings.Size.X - 128, 300),
};

coll1.Load(Renderer);
coll2.Load(Renderer);

while (Renderer.Run())
{
    if (!AABB.Intersects(coll1, coll2))
    {
        coll1.Position = new Vector2(coll1.Position.X + 0.1f, coll1.Position.Y);
        coll2.Position = new Vector2(coll2.Position.X - 0.1f, coll2.Position.Y);
    }

    coll1.Draw(Renderer);
    coll2.Draw(Renderer);

    PrimitiveRenderer.DrawText(Renderer, "Packaging example (files are extracted from package and deleted at shutdown)", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    Renderer.Render();
}