﻿using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color
using System.Numerics;

//Lightning lighting Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Init(args);

Renderer renderer = new();
renderer.Start(new RendererSettings()); // use default Renderersettings

// make the Renderer white to emphasise the light
renderer.Clear(Color.White);
// set the environmental light
LightManager.SetEnvironmentalLight(Color.Black);

Light newLight = new()
{
    Brightness = 140, // range 0-255, (255 - value) = lowest alpha range in environmental light
    Range = 8,
    Position = new Vector2(150, 125)
};

Light newColouredLight = new()
{
    Brightness = 140,
    Range = 5,
    Position = new Vector2(150, 550),
    LightColor = Color.Green
};

Light newColouredLight2 = new()
{
    Brightness = 190,
    Range = 6,
    Position = new Vector2(425, 550),
    LightColor = Color.Green
};

Light newColouredLight3 = new()
{
    Brightness = 255,
    Range = 7,
    Position = new Vector2(750, 550),
    LightColor = Color.Green
};

LightManager.AddAsset(renderer, newLight);
LightManager.AddAsset(renderer, newColouredLight);
LightManager.AddAsset(renderer, newColouredLight2);
LightManager.AddAsset(renderer, newColouredLight3);

while (renderer.Run())
{
    PrimitiveRenderer.DrawText(renderer, "Lighting example", new Vector2(100, 100), Color.Black); // no fonts loaded so we use the debug font
    renderer.Render();
}