using System;
using System.Collections.Generic;
using System.Text;
using FcuCore.Communications;

namespace FcuCore
{
    public class CbusManager
    {
        private CbusConnectionState _connectionState = CbusConnectionState.Disconnected;
        public ICbusMessenger Messenger { get; private set; }

        public CommunicationSettings CommsSettings { get; set; } = new CommunicationSettings();

        public CbusConnectionState ConnectionState {
            get => _connectionState;
            set {
                if (value != _connectionState) {
                    _connectionState = value;
                    ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_connectionState));
                }
            }
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged; 

        public void OpenComms()
        {
            ITransport transport;
            switch (CommsSettings.Transport) {
                case CommunicationSettings.TransportProviders.Test:
                    transport = new FakeTransport();
                    break;
                case CommunicationSettings.TransportProviders.Serial:
                    var spt = new SerialPortTransport(CommsSettings.SerialPort);
                    spt.Open();
                    transport = spt;
                    break;
                default:
                    throw new InvalidOperationException("No valid transport configured");
            }
            Messenger = new CbusMessenger(transport);
            ConnectionState = CbusConnectionState.Connected;
        }

        public void CloseComms()
        {
            ConnectionState = CbusConnectionState.Disconnected;
            Messenger = null;
        }

    }
}
