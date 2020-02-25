using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public abstract class CbusMessageWithNodeNumberAndDeviceNumber : CbusMessageWithNodeNumber
    {
        protected CbusMessageWithNodeNumberAndDeviceNumber(CbusOpCodes opCode) : base(opCode) { }

        public ushort DeviceNumber => (ushort)((Data[2] << 8) + Data[3]);
    }
}