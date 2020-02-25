namespace FcuCore.Communications.Opcodes
{
    [CbusMessage(CbusOpCodes.ASON)]
    public class AsOnMessage : CbusMessageWithNodeNumberAndDeviceNumber
    {
        public AsOnMessage() : base(CbusOpCodes.ASON) { }
        public override string DisplayString => $"Accessory On (Short), DeviceNumber: {DeviceNumber}, Node: {NodeNumber}";
    }

    [CbusMessage(CbusOpCodes.ASOF)]
    public class AsOffMessage : CbusMessageWithNodeNumberAndDeviceNumber
    {
        public AsOffMessage() : base(CbusOpCodes.ASOF) { }
        public override string DisplayString => $"Accessory Off (Short), DeviceNumber: {DeviceNumber}, Node: {NodeNumber}";
    }
}