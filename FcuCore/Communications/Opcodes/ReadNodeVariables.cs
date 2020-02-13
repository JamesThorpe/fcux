using System;
using System.Collections.Generic;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    [CbusMessage(CbusOpCodes.NVRD)]
    public class ReadNodeVariableMessage : CbusMessageWithNodeNumber
    {
        public ReadNodeVariableMessage() : base(CbusOpCodes.NVRD)
        {
            Data = new byte[3];
        }

        public byte VariableIndex {
            get => Data[2];
            set => Data[2] = value;
        }

        public override string DisplayString =>
            $"Read Node Variable, Node Number: {NodeNumber}, Variable Index: {VariableIndex}";
    }

    [CbusMessage(CbusOpCodes.NVANS)]
    public class ReadNodeVariableAnswerMessage : CbusMessageWithNodeNumber
    {
        public ReadNodeVariableAnswerMessage():base(CbusOpCodes.NVANS)
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

        public override string DisplayString =>
            $"Read Variable Answer, Node Number: {NodeNumber}, Variable Index: {VariableIndex}, Value: {VariableValue}";
    }
}
