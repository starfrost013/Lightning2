namespace LightningGL
{
    /// <summary>
    /// DebugViews
    /// 
    /// Enumerates debug page views.
    /// </summary>
    public enum DebugViews
    {
        /// <summary>
        /// <para>"Big picture" debug view:</para>
        /// <para>- how many renderables are there?</para>
        /// <para>- how many are on screen?</para>
        /// <para>- engine versioning information</para>
        /// <para>- framerate / frametime </para>
        /// <para>- Are we windowed or not?</para>
        /// </summary>
        BigPicture = 0,

        /// <summary>
        /// <para>Detailed information about renderables</para>
        /// <para>Giant list of their names etc</para>
        /// </summary>
        RenderableDetails = 1,

        /// <summary>
        /// <para>System information.</para>
        /// <para>The contents of the <see cref="SystemInfo"/> class.</para>
        /// </summary>
        SystemInformation = 2,

        /// <summary>
        /// <para>Detailed performance information.</para>
        /// <para>- Gen0/1/2 GC</para>
        /// <para>- RAM/CPU use</para>
        /// </summary>
        Performance = 3,

        /// <summary>
        /// <para>View GlobalSettings values.</para>
        /// </summary>
        GlobalSettings = 4,

        /// <summary>
        /// Sentinel value for the maximum page.
        /// </summary>
        MaxPage = GlobalSettings,
    }
}