using System;
using System.Threading.Tasks;

namespace FcuCore.Communications
{
    public interface ICbusMessenger
    {
        event EventHandler<CbusMessageEventArgs> MessageReceived;

        Task<bool> SendMessage(CbusMessage message);
    }
}