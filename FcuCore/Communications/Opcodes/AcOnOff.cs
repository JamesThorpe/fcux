namespace FcuCore.Communications.Opcodes {
    [CbusMessage(CbusOpCodes.ACON)]
    public class AcOnMessage : CbusMessageWithNodeNumberAndEvent
    {
        public AcOnMessage():base(CbusOpCodes.ACON) {}
        public override string DisplayString => $"Accessory On, Node {NodeNumber}, Event {EventNumber}";
    }

    [CbusMessage(CbusOpCodes.ACOF)]
    public class AcOffMessage : CbusMessageWithNodeNumberAndEvent
    {
        public AcOffMessage() : base(CbusOpCodes.ACOF) { }
        public override string DisplayString => $"Accessory Off, Node {NodeNumber}, Event {EventNumber}";
    }
}