using System;
using System.Collections.Generic;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    [CbusMessage(CbusOpCodes.NVRD)]
    public class MessageReadNodeVariables:MessageNode
    {
        public MessageReadNodeVariables() : base(CbusOpCodes.NVRD)
        {
            Data = new byte[3];
        }

        public byte VariableIndex {
            get => Data[2];
            set => Data[2] = value;
        }
    }

    [CbusMessage(CbusOpCodes.NVANS)]
    public class MessageReadNodeVariablesAnswer : MessageNode
    {
        public MessageReadNodeVariablesAnswer():base(CbusOpCodes.NVANS)
        {
            Data = new byte[4];
        }

        public byte VariableIndex {
            get => Data[2];
            set => Data[2] = value;
        }
        public byte VariableValue {
            get => Data[3];
            set => Data[3] = value;
        }
    }
}
