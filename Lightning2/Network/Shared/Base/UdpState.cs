namespace LightningGL
{
    /// <summary>
    /// <para>UDP internal message state</para>
    /// 
    /// <para>required to get IP Address of client connecting to server</para>
    /// </summary>
    internal class UdpState
    {
        /// <summary>
        /// <see cref="UdpClient"/> object representing the connected client.
        /// </summary>
        public UdpClient? Client { get; set; }

        /// <summary>
        /// IP address of the client machine.
        /// </summary>
        public IPEndPoint? Ip { get; set; }
    }
}
