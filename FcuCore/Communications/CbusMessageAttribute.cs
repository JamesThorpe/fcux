using System;
using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    [AttributeUsage(AttributeTargets.Class)]
    public class CbusMessageAttribute:Attribute
    {
        public CbusOpCodes OpCode { get; }

        public CbusMessageAttribute(CbusOpCodes opCode)
        {
            OpCode = opCode;
        }
    }
}