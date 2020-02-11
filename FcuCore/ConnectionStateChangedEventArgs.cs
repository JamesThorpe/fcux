namespace FcuCore {
    public class ConnectionStateChangedEventArgs
    {
        public CbusConnectionState ConnectionState { get; }

        public ConnectionStateChangedEventArgs(CbusConnectionState connectionState)
        {
            ConnectionState = connectionState;
        }
    }
}