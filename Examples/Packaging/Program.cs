using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Lightning collision example (Scene Manager NOT used)
//©2022 starfrost, August 13, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings
{
    Title = "Basic Lightning2 Demo"
}); // use default windowsettings except title

Texture coll1 = new Texture(window, 128, 128)
{
    Path = "Content/CollidingTexture1.png",
    Position = new Vector2(128, 300),
};

Texture coll2 = new Texture(window, 128, 128)
{
    Path = "Content/CollidingTexture2.png",
    Position = new Vector2(window.Settings.Size.X - 128, 300),
};

coll1.Load(window);
coll2.Load(window);

while (window.Run())
{
    if (!AABB.Intersects(coll1, coll2))
    {
        coll1.Position = new Vector2(coll1.Position.X + 0.1f, coll1.Position.Y);
        coll2.Position = new Vector2(coll2.Position.X - 0.1f, coll2.Position.Y);
    }

    coll1.Draw(window);
    coll2.Draw(window);

    PrimitiveRenderer.DrawText(window, "Packaging example (files extracted from package)", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}