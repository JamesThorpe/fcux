using System;
using System.Threading.Tasks;

namespace FcuCore.Communications {
    public interface ITransport
    {
        event EventHandler<BytesReceivedEventArgs> BytesReceived;

        Task SendBytes(byte[] bytes);
    }

    public class BytesReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; }

        public BytesReceivedEventArgs(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}