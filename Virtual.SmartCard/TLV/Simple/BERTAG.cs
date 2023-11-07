namespace Virtual.SmartCard.TLV.Simple
{
    public class BERTAG
    {
        public BERTAG(BERClass @class, BEREncodingForm encodingForm, BERType type, uint tagNumber)
        {
            Class = @class;
            EncodingForm = encodingForm;
            Type = type;
            TagNumber = tagNumber;
        }

        public BERClass Class { get; private set; }
        public BEREncodingForm EncodingForm { get; private set; }
        public BERType Type { get; private set; }
        public uint TagNumber { get; private set; }
    }
}