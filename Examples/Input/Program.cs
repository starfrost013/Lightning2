using LightningGL; // use lightninggl
using static LightningBase.SDL; // import for sdl
using System.Drawing; // for color
using System.Numerics;

//Lightning Input Example (Scene Manager NOT used)
//©2022 starfrost, August 12, 2022

// Initialise Lightning
Lightning.Init(args);

Renderer Renderer = new Renderer();
Renderer.Start(new RendererSettings()); // use default Renderersettings

string youPressedString = "Last keypress: ";
string youClickedString = "Last mouse click: ";
string lastMousePosString = "Last mouse motion: ";
while (Renderer.Run())
{
    if (Renderer.EventWaiting)
    {
        if (Renderer.LastEvent.type == SDL_EventType.SDL_KEYDOWN)
        {
            Key key = (Key)Renderer.LastEvent.key;
            youPressedString = $"Last keypress: {key}, modifiers: {key.Modifiers}, repeated: {key.Repeated}";
        }
        else if (Renderer.LastEvent.type == SDL_EventType.SDL_MOUSEBUTTONDOWN)
        {
            MouseButton button = (MouseButton)Renderer.LastEvent.button;
            youClickedString = $"Last mouse click: {button.Button}, position: {button.Position}";
        }
        else if (Renderer.LastEvent.type == SDL_EventType.SDL_MOUSEMOTION)
        {
            MouseButton button = (MouseButton)Renderer.LastEvent.motion;
            lastMousePosString = $"Last mouse motion: {button.Button}, position: {button.Position}, velocity: {button.Velocity}";
        }
    }

    PrimitiveRenderer.DrawText(Renderer, "Input example", new Vector2(100, 100), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(Renderer, youPressedString, new Vector2(100, 120), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(Renderer, youClickedString, new Vector2(100, 140), Color.White); // no fonts loaded so we use the debug font
    PrimitiveRenderer.DrawText(Renderer, lastMousePosString, new Vector2(100, 160), Color.White); // no fonts loaded so we use the debug font
    Renderer.Render();
}