using LightningGL; // use lightninggl
using System;
using System.Drawing; // for color
using System.Numerics;

//Basic Lightning Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

// create a texture
Texture texture = new Texture(window, 512, 512);
Random Random = new Random();

while (window.Run())
{
    int rX = Random.Next(0, 512); // exclusive upper bound
    int rY = Random.Next(0, 512);

    // you shouldn't lock/unlock constantly, it's not very efficient
    // this is solely done for the purpose of this demo ONLY
    texture.SetPixel(rX, rY, Color.White);

    texture.Draw(window);

    PrimitiveRenderer.DrawText(window, "Texture API example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    window.Render();
}