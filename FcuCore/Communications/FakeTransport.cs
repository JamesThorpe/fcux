using System;
using System.Text;
using System.Threading.Tasks;

namespace FcuCore.Communications {
    public class FakeTransport : ITransport {
        public event EventHandler<BytesReceivedEventArgs> BytesReceived;
        public async Task SendBytes(byte[] bytes)
        {
            
        }

        public FakeTransport()
        {
            Task.Run(async () => {
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