using System;
using System.Collections.Generic;
using System.Text;

namespace FcuCore.Communications.Opcodes
{
    //send:
    //:SBFA0N0D;

    //receive:
    //:SB060NB60102A5080D - acc4
    //:SB020NB60100A5050F - ace8c
    //:SB040NB60101A5050F - ace8c

    [CbusMessage(0x0D)]
    public class MessageQueryAllNodes:CbusMessage
    {
        public override string DisplayString => "Query all nodes";
    }
}
