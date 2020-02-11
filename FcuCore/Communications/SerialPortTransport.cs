using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FcuCore.Communications
{
    public class SerialPortTransport :ITransport, IDisposable
    {
        private readonly string _portName;
        private SerialPort _port;
        private CancellationTokenSource _cts;
        public SerialPortTransport(string portName)
        {
            _portName = portName;
        }

        public async Task Open()
        {
            _cts = new CancellationTokenSource();
            _port = new SerialPort(_portName);
            try {
                _port.Open();
            } catch (Exception e) {

            }

            var buffer = new byte[30];
            while (!_cts.IsCancellationRequested) {
                try {
                    var read = await _port.BaseStream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                    BytesReceived?.Invoke(this, new BytesReceivedEventArgs(buffer.Take(read).ToArray()));
                } catch (TaskCanceledException) {
                    //ok
                }
            }
        }

        public void Dispose()
        {
            _port?.Dispose();
            _port = null;
            _cts.Cancel();
            _cts?.Dispose();
        }

        public event EventHandler<BytesReceivedEventArgs> BytesReceived;
        public async Task SendBytes(byte[] bytes)
        {
            try {
                await _port.BaseStream.WriteAsync(bytes, 0, bytes.Length, _cts.Token);
            } catch (TaskCanceledException) {
                //ok
            }
        }
    }
}
