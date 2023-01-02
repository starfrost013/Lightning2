namespace LightningGL
{
    /// <summary>
    /// AnimationPropertyFactory
    /// 
    /// Defines animation property value calculators for various types of animation properties.
    /// </summary>
    internal static class AnimationPropertyFactory
    {
        internal static int GetIntValue(int int1, int int2, long cur, long max) => int1 + ((int2 - int1) * (int)((double)cur / max));

        internal static double GetDoubleValue(double double1, double double2, long cur, long max) => double1 + ((double2 - double1) * ((double)cur / max));

        internal static float GetFloatValue(float float1, float float2, long cur, long max) => float1 + ((float2 - float1) * ((float)cur / max));

        internal static Vector2 GetVector2Value(Vector2 vec1, Vector2 vec2, long cur, long max) => vec1 + ((vec2 - vec1) * ((float)cur / max));

        internal static bool GetBooleanValue(bool bool1, bool bool2, long cur, long max) => ((double)cur / max) > 0.5 ? bool2 : bool1; 
    }
}
