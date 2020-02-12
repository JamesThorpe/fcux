using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    [CbusMessage(CbusOpCodes.RQNPN)]
    public class MessageRequestReadOfNodeParameterByIndex: MessageNode
    {
        public MessageRequestReadOfNodeParameterByIndex() : base(CbusOpCodes.RQNPN) { }

        public byte ParameterIndex
        {
            get => Data[2];
            set => Data[2] = value;
        }
    }


    [CbusMessage(CbusOpCodes.PARAN)]
    public class MessageNodeParameterResponse : MessageNode
    {
        public MessageNodeParameterResponse(CbusOpCodes opCode) : base(opCode) { }

        public byte ParameterIndex
        {
            get => Data[2];
            set => Data[2] = value;
        }

        public byte ParameterValue
        {
            get => Data[3];
            set => Data[3] = value;
        }
    }

}
