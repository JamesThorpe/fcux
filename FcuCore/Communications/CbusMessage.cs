using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FcuCore.Communications {
    public class CbusMessage {
        public enum FrameTypes
        {
            Normal,
            Rtr
        }

        public string TransportString { get; private set; }
        public FrameTypes FrameType { get; private set; }
        public byte[] Data { get; private set; }
        public byte SidH { get; private set; }
        public byte SidL { get; private set; }
        public byte OpCode { get; private set; }

        public virtual string DisplayString => $"Unknown {OpCode}, Data {BitConverter.ToString(Data)}";

        public override string ToString()
        {
            return DisplayString;
        }

        public static CbusMessage FromTransportString(string transportString)
        {
   

            //:SB020N9101000005;

            var p = 1;
            if (transportString[p] != 'S') {
                //non-standard, don't support yet
                throw new NotImplementedException();
            }

            p++;
            var sidh = Convert.ToByte(transportString.Substring(p,2), 16);
            p += 2;
            var sidl = Convert.ToByte(transportString.Substring(p, 2), 16);
            p += 2;
            var frametype = transportString[p] == 'N' ? FrameTypes.Normal : FrameTypes.Rtr;
            p++;

            var dataBytes = new byte[(transportString.Length-1-p)/2];
            for (var x = 0; p < transportString.Length-1; p+=2, x++) {
                dataBytes[x] = Convert.ToByte(transportString.Substring(p, 2), 16);
            }

            var opCode = dataBytes[0];

            var opcodeType = GetOpCodeType(opCode);
            CbusMessage msg;
            if (opcodeType == null) {
                msg = new CbusMessage();
            } else {
                msg = (CbusMessage) Activator.CreateInstance(opcodeType);
            }

            msg.TransportString = transportString;
            msg.SidH = sidh;
            msg.SidL = sidl;
            msg.FrameType = frametype;
            msg.OpCode = opCode;
            msg.Data = dataBytes.Skip(1).ToArray();

            return msg;
        }


        static Type GetOpCodeType(byte opCode)
        {
            var assembly = Assembly.GetAssembly(typeof(CbusMessage));
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(CbusMessageAttribute), true).Length > 0) {
                    if (type.GetCustomAttribute<CbusMessageAttribute>().OpCode == opCode) {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}