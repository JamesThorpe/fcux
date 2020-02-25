using System;
using System.Collections.Generic;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    public enum CbusOpCodes
    {
        QNN = 0x0D,
        NVRD = 0x71,
        RQNPN = 0x73,
        ACON = 0x90,
        ACOF = 0x91,
        NVANS = 0x97,
        ASON = 0x98,
        ASOF = 0x99,
        PARAN = 0x9B,
        PNN = 0xB6,
    }
}
