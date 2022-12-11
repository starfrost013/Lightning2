namespace LightningGL
{
    /// <summary>
    /// Bezier
    /// 
    /// Renders a bezier curve.
    /// </summary>
    public class Bezier : Primitive
    {
        private const int BEZIER_MINIMUM_STEPS = 2; 

        public Bezier(string name) : base(name)
        {
            Points = new();
        }

        public List<Vector2> Points { get; set; }

        private int _steps;
        public int Steps
        {
            get
            {
                return _steps;
            }
            set
            {
                if (value < BEZIER_MINIMUM_STEPS)
                {
                    NCError.ShowErrorBox($"Attempted to set bezier curve steps value below 2, 2 will be used!", 232, "Bezier::Steps::set called with value below 2!",
                        NCErrorSeverity.Warning, null, true);
                    _steps = 2;
                    return;
                }
                _steps = value;
            }
        }


        internal override void Draw()
        {
            int[] vx = new int[Points.Count];
            int[] vy = new int[Points.Count];

            for (int pointId = 0; pointId < Points.Count; pointId++)
            {
                vx[pointId] = ((int)Points[pointId].X);
                vy[pointId] = ((int)Points[pointId].Y);   

            }

            if (BorderSize.X > 0
                || BorderSize.Y > 0)
            {
                int[] borderVx = new int[Points.Count];
                int[] borderVy = new int[Points.Count];

                Buffer.BlockCopy(vx, 0, borderVx, 0, sizeof(int) * borderVx.Length);
                Buffer.BlockCopy(vy, 0, borderVy, 0, sizeof(int) * borderVy.Length);

                // not the best way of doing this
                for (int xId = 0; xId < borderVx.Length; xId++) borderVx[xId] -= (int)BorderSize.X;
                for (int yId = 0; yId < borderVy.Length; yId++) borderVx[yId] -= (int)BorderSize.Y;

                Lightning.Renderer.DrawBezier(vx, vy, Steps, BorderColor.R, BorderColor.G, BorderColor.B, BorderColor.A);
            }

            Lightning.Renderer.DrawBezier(vx, vy, Steps, Color.R, Color.G, Color.B, Color.A);
        }
    }
}
