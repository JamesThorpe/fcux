using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public abstract class MessageNodeAndEvent : MessageNode
    {
        protected MessageNodeAndEvent(CbusOpCodes opCode):base(opCode) { }
        
        public ushort EventNumber => (ushort)((Data[2] << 8) + Data[3]);
    }
}