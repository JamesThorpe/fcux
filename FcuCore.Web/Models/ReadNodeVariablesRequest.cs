using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FcuCore.Web.Models
{
    public class ReadNodeVariablesRequest
    {
        public ushort NodeNumber { get; set; }
        public byte VariableCount { get; set; }
    }
}
