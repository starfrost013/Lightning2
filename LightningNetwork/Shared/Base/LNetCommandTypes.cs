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
        PingPong = 0x00,

        /// <summary>
        /// Client keepalive to the server.
        /// </summary>
        KeepAlive = 0x01,

        /// <summary>
        /// Client says hello to the server
        /// </summary>
        ClientHello = 0x02,

        /// <summary>
        /// Client disconnecting
        /// </summary>
        ClientDisconnecting = 0x03,
        
        /// <summary>
        /// Client has disconnected.
        /// </summary>
        ClientDisconnected = 0x04,

        /// <summary>
        /// Client wants to change room.
        /// </summary>
        ClientChangeRoomRequest = 0x05,

        /// <summary>
        /// Client was kicked from the game.
        /// </summary>
        LeaveNowOrDie = 0x06,

        /// <summary>
        /// Server acknowledges client's existence.
        /// </summary>
        ServerHello = 0x07,

        /// <summary>
        /// Notify a server shutdown.
        /// </summary>
        ServerNotifyShutdown = 0x08,

        /// <summary>
        /// Client wants to add a renderable to the scene.
        /// </summary>
        RequestRenderable = 0x09,

        /// <summary>
        /// Maximum valid command for logging.
        /// </summary>
        MaximumValidCommand = RequestRenderable,
    }
}
