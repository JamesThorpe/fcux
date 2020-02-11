using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {

    public abstract class MessageNodeAndEvent : CbusMessage
    {
        protected MessageNodeAndEvent(CbusOpCodes opCode):base(opCode)
        {
            
        }
        public int NodeNumber => (Data[0] << 8) + Data[1];
        public int EventNumber => (Data[2] << 8) + Data[3];
    }

    [CbusMessage(CbusOpCodes.AcOn)]
    public class MessageAcOn : MessageNodeAndEvent
    {
        public MessageAcOn():base(CbusOpCodes.AcOn) {}
        public override string DisplayString => $"ACON Node {NodeNumber}, Event {EventNumber}";
    }

    [CbusMessage(CbusOpCodes.AcOff)]
    public class MessageAcOff : MessageNodeAndEvent
    {
        public MessageAcOff() : base(CbusOpCodes.AcOff) { }
        public override string DisplayString => $"ACOFF Node {NodeNumber}, Event {EventNumber}";
    }
}