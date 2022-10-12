namespace LightningGL
{
    /// <summary>
    /// AnimationPropertyFactory
    /// 
    /// September 17, 2022
    /// 
    /// Defines animation property value calculators for various types of animation properties.
    /// </summary>
    internal static class AnimationPropertyFactory
    {
        internal static int GetIntValue(object obj1, object obj2, long cur, long max)
        {
            int int1 = Convert.ToInt32(obj1);
            int int2 = Convert.ToInt32(obj2);

            return int1 + ((int2 - int1) * Convert.ToInt32((double)cur / max));
        }

        internal static double GetDoubleValue(object obj1, object obj2, long cur, long max)
        {
            double double1 = Convert.ToDouble(obj1);
            double double2 = Convert.ToDouble(obj2);

            return double1 + ((double2 - double1) * ((double)cur / max));
        }

        internal static float GetFloatValue(object obj1, object obj2, long cur, long max)
        {
            float float1 = Convert.ToSingle(obj1);
            float float2 = Convert.ToSingle(obj2);

            return float1 + ((float2 - float1) * ((float)cur / max));
        }

        internal static Vector2 GetVector2Value(object obj1, object obj2, long cur, long max)
        {
            Vector2 vec1 = (Vector2)obj1;
            Vector2 vec2 = (Vector2)obj2;

            return vec1 + ((vec2 - vec1) * ((float)cur / max));
        }
    }
}
