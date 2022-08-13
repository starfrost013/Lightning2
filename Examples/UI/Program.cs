using LightningGL; // use lightninggl
using System.Drawing; // for color

//Basic Lightning UI Example 
//©2022 starfrost, August 13, 2022

// Initialise Lightning
Lightning.Init(args);

Window window = new Window();
window.Start(new WindowSettings
{
    Title = "Lightning UI demonstration"
}); // use default windowsettings except title

Button button1 = new Button() // 150 char capacirty
{
    Size = new(100, 30),
    BackgroundColour = Color.White,
    PressedColour = Color.Gray,
    HoverColour = Color.LightGray,
    BorderColour = Color.Yellow,
    BorderSize = new(3, 3),
    ForegroundColour = Color.Black,
    Position = new(100, 300),
    Text = "Button 1",
    Filled = true
    
};

// register event handlers
button1.OnMousePressed += OnMousePressed;
button1.OnMouseReleased += OnMouseReleased;

UIManager.AddElement(button1);

string buttonHeldDownText = "Button not held down";

while (window.Run())
{
    PrimitiveRenderer.DrawText(window, "UI example", new(100, 100), Color.White); // no fonts loaded so we use the debug font - new is vector2
    PrimitiveRenderer.DrawText(window, buttonHeldDownText, new(100, 120), Color.White);
    window.Render();
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