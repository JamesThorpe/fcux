using System;

namespace FcuCore.Communications {
    [AttributeUsage(AttributeTargets.Class)]
    public class CbusMessageAttribute:Attribute
    {
        public byte OpCode { get; }

        public CbusMessageAttribute(byte opCode)
        {
            OpCode = opCode;
        }
    }
}