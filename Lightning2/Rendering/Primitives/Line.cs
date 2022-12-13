namespace LightningGL
{
    public class Line : Primitive
    {
        public Vector2 Start { get; set; }

        public Vector2 End { get; set; }

        public Line(string name) : base(name) { }

        internal override void Draw()
        {
            // lineRGBA(); just calls SDL.SDL_RenderDrawLine
            // thickLine does other stuff. 
            // therefore call lineRGBA if thickness = 1

            // 2022-02-25: Changed SDL2_gfx in C++
            // to support 16-bit thickness instead of 8-bit

            // nobody will ever need a line more than 32,767 pixels wide
            // (he says, regretting this in the future). If we do we can just change to sint32 in c++.

            // before we manually called lineRGBA. this is now done in c++, so we don't need to.

            if (BorderSize.X > 0
                && BorderSize.Y > 0)
            {
                Lightning.Renderer.DrawLine((int)RenderPosition.X - (int)BorderSize.X, (int)RenderPosition.Y - (int)BorderSize.Y, 
                    (int)RenderPosition.X + (int)Size.X + (int)BorderSize.X, (int)RenderPosition.Y + (int)Size.Y + (int)BorderSize.Y, 
                    BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            Lightning.Renderer.DrawLine((int)RenderPosition.X, (int)RenderPosition.Y, (int)RenderPosition.X + (int)Size.X, (int)RenderPosition.Y + (int)Size.Y, 
                Color.R, Color.G, Color.B, Color.A);
        }
    }
}
