using LightningGL; // use lightninggl
using System.Drawing; // for color
using System.Numerics;

//Basic Lightning Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings()); // use default windowsettings

while (window.Run())
{
    // #[name] is used for localisation strings
    PrimitiveRenderer.DrawText(window, "#[LOC_STRING_01]", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "#[LOC_STRING_02]", new Vector2(100, 150), Color.Red); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(window, "#[LOC_STRING_03]", new Vector2(100, 200), Color.RebeccaPurple); // no fonts loaded so we use the debug font
    window.Render();
}