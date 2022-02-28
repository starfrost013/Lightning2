// See https://aka.ms/new-console-template for more information

// Lightning2 Test Launcher

// Test Lightning2 application
// © 2022 starfrost
using NuCore.SDL2;
using NuCore.Utilities;
using Lightning2;

Lightning2.Lightning2.Init(); // Initialise Lightning2

Window Window = new Window();
WindowSettings WS = new WindowSettings();

WS.Title = "Lightning2 Example";
WS.Position = new Vector2(350, 100);
WS.Size = new Vector2(960, 640);

Window.AddWindow(WS);

Texture Texture = new Texture(256, 256);

Texture.Create(Window);

Random Rnd = new Random();

// TextureAPI test
byte R = (byte)Rnd.Next(0, 256);
byte G = (byte)Rnd.Next(0, 256);
byte B = (byte)Rnd.Next(0, 256);

for (int x = 0; x < 256; x++)
{

    for (int y = 0; y < 256; y++)
    {
        B += (byte)Rnd.Next(-5, 5);
        G += (byte)Rnd.Next(-5, 5);
        B += (byte)Rnd.Next(-5, 5);
        Texture.SetPixel(x, y, new Color4(R, G, B, 255));
    }
}

Texture.Unlock();

bool Running = true;

while (Running)
{
    Window.Update();

    SDL.SDL_Event Event = new SDL.SDL_Event();

    if (SDL.SDL_PollEvent(out Event) > 0) 
    {
        switch (Event.type)
        {
            case SDL.SDL_EventType.SDL_QUIT:
                Running = false;
                return;
            case SDL.SDL_EventType.SDL_KEYDOWN:
                continue; // TEMP Code
        }
    }
    else
    {
        SDL.SDL_Rect Src = new SDL.SDL_Rect(0, 0, 256, 256);
        SDL.SDL_Rect Dst = new SDL.SDL_Rect(0, 0, 256, 256);

        SDL.SDL_RenderCopy(Window.Settings.RendererHandle, Texture.TextureHandle, ref Src, ref Dst);

        Window.Present();
    }
}
