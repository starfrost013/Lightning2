using LightningGL; // use lightninggl
using static LightningGL.Lightning;
using System.Drawing; // for color

//Basic Lightning UI Example 
// © 2022-2023 starfrost, March 21, 2023

// Initialise Lightning
InitClient();

Renderer renderer = new Renderer();
renderer.Start(new RendererSettings
{
    Title = "Lightning UI demonstration"
}); // use default Renderersettings except title

Button button1 = new Button() // 150 char capacirty
{
    Size = new(100, 30),
    BackgroundColor = Color.White,
    PressedColor = Color.Gray,
    HoverColor = Color.LightGray,
    BorderColor = Color.Yellow,
    BorderSize = new(3, 3),
    ForegroundColor = Color.Black,
    Position = new(100, 300),
    Text = "Button 1",
    Filled = true
    
};

// register event handlers
button1.OnMousePressed += OnMousePressed;
button1.OnMouseReleased += OnMouseReleased;

UIManager.AddAsset(renderer, button1);

string buttonHeldDownText = "Button not held down";

while (renderer.Run())
{
    PrimitiveRenderer.DrawText(renderer, "UI example", new(100, 100), Color.White); // no fonts loaded so we use the debug font - new is vector2
    PrimitiveRenderer.DrawText(renderer, buttonHeldDownText, new(100, 120), Color.White);
    renderer.Render();
}

void OnMousePressed(MouseButton button)
{
    // call base handler
    button1.MousePressed(button);
    buttonHeldDownText = "Button held down";
}

void OnMouseReleased(MouseButton button)
{
    // call base handler
    button1.MouseReleased(button);
    buttonHeldDownText = "Button not held down";

}