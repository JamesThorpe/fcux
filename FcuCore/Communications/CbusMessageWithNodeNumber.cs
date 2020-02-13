using System.Runtime.InteropServices.ComTypes;
using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    public abstract class CbusMessageWithNodeNumber : CbusMessage
    {
        protected CbusMessageWithNodeNumber(CbusOpCodes opCode) : base(opCode) { }

        public ushort NodeNumber {
            get => (ushort) ((Data[0] << 8) + Data[1]);
            set {
                Data[0] = (byte)(value >> 8);
                Data[1] = (byte) value;
            }
        }
    }
}