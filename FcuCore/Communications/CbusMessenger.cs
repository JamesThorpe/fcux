using System;
using System.Text;
using System.Threading.Tasks;

namespace FcuCore.Communications
{
    public class CbusMessenger : ICbusMessenger
    {
        private readonly ITransport _transport;
        public event EventHandler<CbusMessageEventArgs> MessageReceived;
        public event EventHandler<CbusMessageEventArgs> MessageSent;

        public CbusMessenger(ITransport transport)
        {
            _transport = transport;
            transport.BytesReceived += BytesReceived;
        }

        private string _incomingStream = "";
        private void BytesReceived(object sender, BytesReceivedEventArgs e)
        {
            _incomingStream += Encoding.ASCII.GetString(e.Bytes);
            if (!_incomingStream.StartsWith(":")) {
                //throw away part message
                var i = _incomingStream.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (i > 0) {
                    _incomingStream = _incomingStream.Substring(i);
                }
            }

            while (_incomingStream.StartsWith(":") && _incomingStream.Contains(";")) {
                var end = _incomingStream.IndexOf(";", StringComparison.InvariantCultureIgnoreCase);
                if (end > 0) {
                    var msg = _incomingStream.Substring(0, end + 1);
                    if (end + 1 < _incomingStream.Length) {
                        _incomingStream = _incomingStream.Substring(end + 1);
                    } else {
                        _incomingStream = "";
                    }
                    MessageReceived?.Invoke(this, new CbusMessageEventArgs(CbusMessage.FromTransportString(msg)));
                }
            }
        }

        public async Task<bool> SendMessage(CbusMessage message)
        {
            //TODO: make configurable
            message.CanId = 125;
            message.MinorPriority = MinorPriority.Normal;
            message.MajorPriority = MajorPriority.Low;

            await _transport.SendBytes(Encoding.ASCII.GetBytes(message.TransportString));
            MessageSent?.Invoke(this, new CbusMessageEventArgs(message));
            return true;
        }

    }
}