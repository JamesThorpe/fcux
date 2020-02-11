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
            Messenger = new CbusMessenger(new FakeTransport());
            //var t = new SerialPortTransport("COM5");
            //t.Open();
            //Messenger = new CbusMessenger(t);
            ConnectionState = CbusConnectionState.Connected;
        }

        public void CloseComms()
        {
            ConnectionState = CbusConnectionState.Disconnected;
            Messenger = null;
        }

    }
}
