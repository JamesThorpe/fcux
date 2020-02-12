using FcuCore.Communications.Opcodes;

namespace FcuCore.Communications {
    [CbusMessage(CbusOpCodes.ResponseToQueryNode)]
    public class MessageResponseToQueryNode:MessageNode
    {
        public MessageResponseToQueryNode() : base(CbusOpCodes.ResponseToQueryNode) { }

        public byte ManufacturerId => Data[2];
        public byte ModuleId => Data[3];

        public bool IsConsumerNode => (Data[4] & 0b0001) == 0b0001;
        public bool IsProducerNode => (Data[4] & 0b0010) == 0b0010;
        public bool InFlimMode => (Data[4] & 0b0100) == 0b0100;
        public bool SupportsBootloader => (Data[4] & 0b1000) == 0b1000;

        public override string DisplayString => $"Query Node Response, Manufacturer Id {ManufacturerId}, Module Id {ModuleId}";
    }
}