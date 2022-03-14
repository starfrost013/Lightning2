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

TextManager.LoadFont("Arial", 11, "Arial.11pt");
TextManager.LoadFont("Arial", 18, "Arial.18pt");
TextManager.LoadFont("Arial", 24, "Arial.24pt");
TextManager.LoadFont("Arial", 36, "Arial.36pt");

Texture Texture = new Texture(64, 64);

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

Texture.Position = new Vector2(0, 0);
Texture.Repeat = new Vector2(3, 3);

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

    PrimitiveRenderer.DrawText(Window, "#{STRING_TEST}", new Vector2(500, 90), new Color4(0, 0, 255, 255));
    PrimitiveRenderer.DrawText(Window, "Loc string test: #{STRING_TEST}", new Vector2(500, 120), new Color4(0, 0, 255, 255));
    PrimitiveRenderer.DrawText(Window, "Loc string test: #{STRING_TEST} aaaaaa", new Vector2(500, 150), new Color4(0, 0, 255, 255));

    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 10), new Color4(255, 255, 255, 255));
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 30), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 50), new Color4(255, 255, 0, 255), SDL_ttf.TTF_FontStyle.Italic, new Color4(255, 0, 0, 255));
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 70), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Underline, new Color4(255, 0, 0, 255));
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 90), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Strikeout, new Color4(255, 0, 0, 255), 15);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 110), new Color4(0, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold, new Color4(255, 0, 0, 255), -1, 3);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 130), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold, new Color4(255, 0, 0, 255), 15, -1);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 150), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold, new Color4(255, 0, 0, 255), -1, -1, 30);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 170), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold, new Color4(255, 0, 0, 255), -1, -1, 0, FontSmoothingType.Shaded);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 190), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold, new Color4(255, 0, 0, 255), -1, -1, 0, FontSmoothingType.Solid);
    TextManager.DrawTextTTF(Window, "Test", "Arial.11pt", new Vector2(700, 210), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);
    TextManager.DrawTextTTF(Window, "#{STRING_TEST}", "Arial.11pt", new Vector2(700, 210), new Color4(255, 255, 255, 255), SDL_ttf.TTF_FontStyle.Bold | SDL_ttf.TTF_FontStyle.Italic | SDL_ttf.TTF_FontStyle.Underline);

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
        Texture.Draw(Window);

        // draw fps on top always
        PrimitiveRenderer.DrawText(Window, $"FPS: {Window.CurFPS}", new Vector2(0, 0), new Color4(255, 255, 255, 255));

        Window.Present();
    }
}
