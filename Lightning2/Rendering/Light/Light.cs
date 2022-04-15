using System.Numerics;

namespace Lightning2
{
    /// <summary>
    /// Light
    /// 
    /// April 7, 2022
    /// 
    /// Defines a light. 
    /// </summary>
    public class Light
    {
        public Vector2 Position { get; set; }

        public uint Brightness { get; set; }

        public double Range { get; set; }

        public bool SnapToScreen { get; set; }

        public Light()
        {
            Brightness = 255;
            Range = 10;
        }
    }
}