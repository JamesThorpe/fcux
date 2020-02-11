using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using FcuCore.Communications.Opcodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FcuCore.Communications {
    public class CbusMessage
    {
        public string TransportString {
            get {
                var ts = new StringBuilder(20);
                ts.Append(":S");
                ts.Append(SidH.ToString("X2"));
                ts.Append(SidL.ToString("X2"));
                ts.Append("N");
                ts.Append(Convert.ToString(((byte)OpCode).ToString("X2")));
                if (Data != null && Data.Length > 0) {
                    foreach (var d in Data) {
                        ts.Append(d.ToString("X2"));
                    }
                }
                ts.Append(";");
                return ts.ToString();
            }
        }

        public FrameTypes FrameType { get; private set; }
        public byte[] Data { get; private set; }
        public byte SidH { get; private set; }
        public byte SidL { get; private set; }
        public CbusOpCodes OpCode { get; }

        public MajorPriority MajorPriority {
            get => (MajorPriority) (SidH >> 6);
            set => SidH = (byte) (((byte)value << 6) + (SidH & 0x3f));
        }

        public MinorPriority MinorPriority {
            get => (MinorPriority) (SidH >> 4 & 0x3);
            set => SidH = (byte) (((byte) value << 4) + (SidH & 0xcf));
        }

        public byte CanId {
            get => (byte) ((((SidH << 8) + SidL) >> 5) & 0x7f);
            set {
                SidH = (byte) ((value >> 3) + (SidH & 0xF0));
                SidL = (byte) ((value << 5) & 0xFF);
            }
        }

        public virtual string DisplayString => $"Unknown {OpCode}, Data {BitConverter.ToString(Data)}";

        public override string ToString()
        {
            return DisplayString;
        }

        public CbusMessage(CbusOpCodes opCode)
        {
            OpCode = opCode;
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
                msg = new CbusMessage((CbusOpCodes)opCode);
            } else {
                msg = (CbusMessage) Activator.CreateInstance(opcodeType);
            }

            msg.SidH = sidh;
            msg.SidL = sidl;
            msg.FrameType = frametype;
            msg.Data = dataBytes.Skip(1).ToArray();

            return msg;
        }


        static Type GetOpCodeType(byte opCode)
        {
            var assembly = Assembly.GetAssembly(typeof(CbusMessage));
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(CbusMessageAttribute), true).Length > 0) {
                    if ((byte)type.GetCustomAttribute<CbusMessageAttribute>().OpCode == opCode) {
                        return type;
                    }
                }
            }

            return null;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FrameTypes
    {
        Normal,
        Rtr
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MajorPriority
    {
        High = 0,       //0x00
        Medium = 1,     //0x01
        Low = 2         //0x10
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MinorPriority
    {
        High = 0,
        AboveNormal = 1,
        Normal = 2,
        Low = 3
    }
}