namespace FcuCore.Communications {

    public abstract class MessageNodeAndEvent : CbusMessage
    {
        public int NodeNumber => (Data[0] << 8) + Data[1];
        public int EventNumber => (Data[2] << 8) + Data[3];
    }

    [CbusMessage(0x90)]
    public class MessageAcOn : MessageNodeAndEvent
    {
        public override string DisplayString => $"ACON Node {NodeNumber}, Event {EventNumber}";
    }

    [CbusMessage(0x91)]
    public class MessageAcOff : MessageNodeAndEvent
    {
        public override string DisplayString => $"ACOFF Node {NodeNumber}, Event {EventNumber}";
    }
}