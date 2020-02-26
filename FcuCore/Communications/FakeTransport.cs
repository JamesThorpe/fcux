using System;
using System.Text;
using System.Threading.Tasks;
using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public class FakeTransport : ITransport {
        public event EventHandler<BytesReceivedEventArgs> BytesReceived;
        public async Task SendBytes(byte[] bytes)
        {
            var _ = Task.Run(async () => {
                var msg = Encoding.ASCII.GetString(bytes);
                var cmsg = CbusMessage.FromTransportString(msg);
                switch (cmsg.OpCode) {
                    case CbusOpCodes.QNN:
                        BytesReceived?.Invoke(this,
                            new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(":SB060NB60102A5080D;")));
                        BytesReceived?.Invoke(this,
                            new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(":SB020NB60100A5050F;")));
                        BytesReceived?.Invoke(this,
                            new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(":SB040NB60101A5050F;")));
                        BytesReceived?.Invoke(this,
                            new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(":SB080NB60103A5200F;")));
                        break;
                    case CbusOpCodes.RQNPN:
                        var m = cmsg as ReadNodeParameterByIndexMessage;
                        BytesReceived?.Invoke(this, new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(new ReadNodeParameterByIndexResponseMessage {
                            NodeNumber = m.NodeNumber,
                            ParameterIndex = 1,
                            ParameterValue = 165
                        }.TransportString)));
                        BytesReceived?.Invoke(this, new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(new ReadNodeParameterByIndexResponseMessage {
                            NodeNumber = m.NodeNumber,
                            ParameterIndex = 3,
                            ParameterValue = (byte)((m.NodeNumber == 256 || m.NodeNumber == 257) ? 8 : (m.NodeNumber == 258 ? 5 : 32))
                        }.TransportString)));
                        BytesReceived?.Invoke(this, new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(new ReadNodeParameterByIndexResponseMessage {
                            NodeNumber = m.NodeNumber,
                            ParameterIndex = 6,
                            ParameterValue = (byte)((m.NodeNumber == 259) ? 127 : 10)
                        }.TransportString)));
                        break;
                }
            });
        }

        public FakeTransport()
        {
            Task.Run(async () => {
                return;
                while (true) {
                    await Task.Delay(1000);
                    BytesReceived?.Invoke(this, new BytesReceivedEventArgs(Encoding.ASCII.GetBytes(GetFakeMessage())));
                }
            });
        }

        private Random _rng = new Random();
        private string GetFakeMessage()
        {
            var op = (byte)_rng.Next(144, 146);
            var nn = (ushort)_rng.Next(0, 65536);
            var ev = (ushort)_rng.Next(0, 16);

            
            return $":SB020N{op:X2}{nn:X4}{ev:X4};";
        }
    }
}