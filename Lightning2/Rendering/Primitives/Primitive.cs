﻿
namespace LightningGL
{
    /// <summary>
    /// Defines a Primitive.
    /// </summary>
    public class Primitive : Renderable
    {
        public Color Color { get; set; }

        public Color BorderColor { get; set; }

        public Vector2 BorderSize { get; set; }

        public bool Filled { get; set; }

        public bool Antialiased { get; set; }
    }
}
