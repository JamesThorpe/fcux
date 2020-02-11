using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
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