namespace Virtual.SmartCard.TLV.Simple
{
    public class BERLength
    {
        public BERLength(BERLengthForm form, uint value)
        {
            Form = form;
            Value = value;
        }

        public BERLengthForm Form { get; private set; }
        public uint Value { get; private set; }
    }
}