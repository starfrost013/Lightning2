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

        /// <summary>
        /// Client says hello to the server
        /// </summary>
        LNET_COMMAND_CLIENT_HELLO = 0x02,

        /// <summary>
        /// Client disconnecting
        /// </summary>
        LNET_COMMAND_CLIENT_DISCONNECTING = 0x03,
        
        /// <summary>
        /// Client has disconnected.
        /// </summary>
        LNET_COMMAND_CLIENT_DISCONNECTED = 0x04,

        /// <summary>
        /// Client wants to change room.
        /// </summary>
        LNET_COMMAND_CLIENT_CHANGE_ROOM_REQUEST = 0x05,

        /// <summary>
        /// Client was kicked from the game.
        /// </summary>
        LNET_COMMAND_LEAVE_NOW_OR_DIE = 0x06,

        /// <summary>
        /// Server acknowledges client's existence.
        /// </summary>
        LNET_COMMAND_SERVER_HELLO = 0x07,

        /// <summary>
        /// Notify a server shutdown.
        /// </summary>
        LNET_COMMAND_SERVER_NOTIFY_SHUTDOWN = 0x08,

        /// <summary>
        /// Maximum valid command for logging.
        /// </summary>
        LNET_MAXIMUM_VALID_COMMAND = LNET_COMMAND_SERVER_NOTIFY_SHUTDOWN,
    }
}
