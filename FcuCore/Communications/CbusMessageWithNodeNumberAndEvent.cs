using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public abstract class CbusMessageWithNodeNumberAndEvent : CbusMessageWithNodeNumber
    {
        protected CbusMessageWithNodeNumberAndEvent(CbusOpCodes opCode):base(opCode) { }
        
        public ushort EventNumber => (ushort)((Data[2] << 8) + Data[3]);
    }
}