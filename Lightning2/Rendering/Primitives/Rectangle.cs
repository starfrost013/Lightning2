﻿namespace LightningGL
{ 
    public class Rectangle : Primitive
    {
        public Rectangle(string name) : base(name)
        {

        }

        internal override void Draw()
        {
            if (BorderColor != default(Color))
            {
                rectangleRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y,
                (int)RenderPosition.X + (int)Size.X + ((int)BorderSize.X * 2), (int)RenderPosition.Y + (int)Size.Y + ((int)BorderSize.Y * 2), Color.R, Color.G, Color.B, Color.A);
            }

            if (Filled)
            {
                boxRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
            else
            {
                rectangleRGBA(Lightning.Renderer.Settings.RendererHandle, (int)RenderPosition.X, (int)RenderPosition.Y,
                    (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, Color.R, Color.G, Color.B, Color.A);
            }
        }
    }
}
