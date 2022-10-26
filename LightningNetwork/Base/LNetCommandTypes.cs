namespace LightningNetwork
{
    /// <summary>
    /// <para>Enumerates in-built Lightning Network Command Types.</para>
    /// <para>These are invoked by a giant factory class in order to avoid having to use reflection.</para>
    /// <para>It's recommended to make your own enum if you are wishing to extend the net command set in your game.</para>
    /// <para>The hope is that the Lightning command set will be generic enough so that you do not have to do this,</para>
    /// <para>but I obviously want it to be extensible.</para>
    /// </summary>
    public enum LNetCommandTypes : byte
    {
        /// <summary>
        /// Test command - ping pong!
        /// </summary>
        LNET_COMMAND_TEST_PING_PONG = 0x00,

        /// <summary>
        /// Client keepalive to the server.
        /// </summary>
        LNET_COMMAND_KEEP_ALIVE = 0x01,


    }
}
