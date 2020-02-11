using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public abstract class MessageNode : CbusMessage
    {
        protected MessageNode(CbusOpCodes opCode) : base(opCode) { }
        public ushort NodeNumber => (ushort)((Data[0] << 8) + Data[1]);
    }
}