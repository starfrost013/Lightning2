namespace LightningGL
{
    public class BasicText : Primitive
    {
        public bool Localise { get; set; }

        public string? Text { get; set; }

        internal override void Draw(Renderer cRenderer)
        {
            // Check for a set camera and move relative to the position of that camera if it is set.
            Camera currentCamera = cRenderer.Settings.Camera;

            if (currentCamera != null
                && !SnapToScreen)
            {
                RenderPosition = new(Position.X - currentCamera.Position.X,
                    Position.Y - currentCamera.Position.Y);
            }
            else
            {
                RenderPosition = Position;
            }

            if (Text == null) Text = $"Localisation failure or null string";

            if (Localise) Text = LocalisationManager.ProcessString(Text);

            // todo: in c++: recompile sdl2_gfx to use sint32, not sint16, and modify pinvoke accordingly
            stringRGBA(cRenderer.Settings.RendererHandle, (short)RenderPosition.X, (short)RenderPosition.Y, Text, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
