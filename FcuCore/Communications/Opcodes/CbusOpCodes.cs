using System;
using System.Collections.Generic;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    public enum CbusOpCodes
    {
        QueryAllNodes = 0x0D,
        NVRD = 0x71,
        RQNPN = 0x73,
        AcOn = 0x90,
        AcOff = 0x91,
        NVANS = 0x97,
        PARAN = 0x9B,
        ResponseToQueryNode = 0xB6,
        
    }
}
