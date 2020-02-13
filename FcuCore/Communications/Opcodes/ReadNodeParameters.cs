using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    [CbusMessage(CbusOpCodes.RQNPN)]
    public class ReadNodeParameterByIndexMessage: CbusMessageWithNodeNumber
    {
        public ReadNodeParameterByIndexMessage() : base(CbusOpCodes.RQNPN)
        {
            Data = new byte[3];
        }

        public byte ParameterIndex
        {
            get => Data[2];
            set => Data[2] = value;
        }

        public override string DisplayString => $"Read Node Parameter, Node Number: {NodeNumber}, Param Index: {ParameterIndex}";
    }


    [CbusMessage(CbusOpCodes.PARAN)]
    public class ReadNodeParameterByIndexResponseMessage : CbusMessageWithNodeNumber
    {
        public ReadNodeParameterByIndexResponseMessage() : base(CbusOpCodes.PARAN)
        {
            Data = new byte[4];
        }

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

        public override string DisplayString =>
            $"Read Parameter Response, Node Number: {NodeNumber}, Param Index: {ParameterIndex}, Param Value: {ParameterValue}";
    }

}
