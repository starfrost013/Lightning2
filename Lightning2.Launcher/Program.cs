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
byte A = (byte)Rnd.Next(0, 256);

for (int x = 0; x < Texture.Size.X; x++)
{
    B += (byte)Rnd.Next(-5, 5);
    G += (byte)Rnd.Next(-5, 5);
    B += (byte)Rnd.Next(-5, 5);
    A += (byte)Rnd.Next(-5, 5);

    for (int y = 0; y < Texture.Size.Y; y++)
    {
        Texture.SetPixel(x, y, new Color4(R, G, B, A));
    }
}

Texture.Unlock();


bool Running = true;



while (Running)
{
    Window.Update();


    PrimitiveRenderer.DrawLine(Window, new Vector2(500, 300), new Vector2(600, 300), 1, new Color4(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(Window, new Vector2(500, 270), new Vector2(600, 270), 3, new Color4(255, 255, 255, 255), true);
    PrimitiveRenderer.DrawLine(Window, new Vector2(500, 240), new Vector2(600, 240), 7, new Color4(255, 255, 255, 255), false);
    PrimitiveRenderer.DrawLine(Window, new Vector2(500, 210), new Vector2(600, 210), 15, new Color4(255, 255, 255, 255), true);

    PrimitiveRenderer.DrawCircle(Window, new Vector2(500, 10), new Vector2(50, 50), new Color4(255, 0, 0, 255), true);
    PrimitiveRenderer.DrawCircle(Window, new Vector2(500, 309), new Vector2(50, 50), new Color4(255, 255, 255, 127), false) ;

    SDL.SDL_Event cur_event = new SDL.SDL_Event();

    if (SDL.SDL_PollEvent(out cur_event) > 0) 
    {
        switch (cur_event.type)
        {
            case SDL.SDL_EventType.SDL_QUIT:
                Running = false;
                return;
            case SDL.SDL_EventType.SDL_KEYDOWN:
                Key nkey = (Key)cur_event.key;
                nkey.Repeated = (cur_event.key.repeat > 0);
                NCLogging.Log($"KeyPress: {nkey}, repeated: {nkey.Repeated}");
                continue; // TEMP Code
        }
    }
    else
    {
        for (int x = 0; x < Texture.Size.X; x++)
        {
            // TextureAPI test
            B += (byte)Rnd.Next(-1, 1);
            G += (byte)Rnd.Next(-1, 1);
            B += (byte)Rnd.Next(-1, 1);
            A += (byte)Rnd.Next(-1, 1);

            for (int y = 0; y < Texture.Size.Y; y++)
            {
                Texture.SetPixel(x, y, new Color4(R, G, B, A));
            }
        }

        Texture.Unlock();

        SDL.SDL_Rect Src = new SDL.SDL_Rect(0, 0, 256, 256);
        SDL.SDL_Rect Dst = new SDL.SDL_Rect(0, 0, 256, 256);

        SDL.SDL_RenderCopy(Window.Settings.RendererHandle, Texture.TextureHandle, ref Src, ref Dst);

        Window.Present();
    }
}
