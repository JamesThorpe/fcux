using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    [CbusMessage(CbusOpCodes.ACON)]
    public class MessageAcOn : MessageNodeAndEvent
    {
        public MessageAcOn():base(CbusOpCodes.ACON) {}
        public override string DisplayString => $"ACON Node {NodeNumber}, Event {EventNumber}";
    }

    [CbusMessage(CbusOpCodes.ACOF)]
    public class MessageAcOff : MessageNodeAndEvent
    {
        public MessageAcOff() : base(CbusOpCodes.ACOF) { }
        public override string DisplayString => $"ACOFF Node {NodeNumber}, Event {EventNumber}";
    }
}