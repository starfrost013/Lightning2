namespace LightningGL
{
    /// <summary>
    /// Renderer
    /// 
    /// August 12, 2022
    /// 
    /// Enumerates all supported SDL renderers.
    /// </summary>
    public enum Renderer
    {
        /// <summary>
        /// Use SDL's default renderer
        /// </summary>
        Default = 0,

        /// <summary>
        /// OpenGL
        /// </summary>
        OpenGL = 1,

        /// <summary>
        /// DirectX 11, or 9.0c if that is not supported
        /// </summary>
        Direct3D = 2,

        /// <summary>
        /// OpenGL ES
        /// </summary>
        OpenGLES = 3,

        /// <summary>
        /// OpenGL ES 2.x
        /// </summary>
        OpenGLES2 = 4,

        /// <summary>
        /// Metal renderer
        /// </summary>
        Metal = 5,

        /// <summary>
        /// Software renderer. HIGHLY NOT RECOMMENDED!
        /// </summary>
        Software = 6
    }
}
