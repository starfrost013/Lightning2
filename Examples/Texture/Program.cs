using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics;

//Texture example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings()); // use default RendererSettings

// create a texture
Texture texture = new Texture(renderer, 512, 512);
TextureManager.AddAsset(renderer, texture);
Random Random = new Random();

while (renderer.Run())
{
    int rX = Random.Next(0, 512); // exclusive upper bound
    int rY = Random.Next(0, 512);

    // you shouldn't lock/unlock constantly, it's not very efficient
    // this is solely done for the purpose of this demo ONLY
    texture.SetPixel(rX, rY, Color.White);

    PrimitiveRenderer.DrawText(renderer, "Texture API example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    renderer.Render();
}