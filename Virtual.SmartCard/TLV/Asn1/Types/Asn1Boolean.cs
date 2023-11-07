using System.IO;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1Boolean : IAsn1Type<bool>
    {
        public Asn1Boolean()
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Boolean);
            Length = new Asn1Length(Asn1LengthForm.Short, 1);
        }

        public Asn1Boolean(bool value)
            : this()
        {
            Value = value;
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public bool Value { get; private set; }

        public void Encode(Stream output)
        {
            output.WriteByte(Value ? (byte)0xFF : (byte)0x00);

            Length = new Asn1Length(Asn1LengthForm.Short, 1);
        }

        public void Decode(Stream input)
        {
            int readByte = input.ReadByte();
            if (readByte == -1)
            {
                throw new Asn1FormatException("Asn1Boolean is not encoded corrected!");
            }

            Value = ((byte) readByte & 0xFF) != 0;
        }
    }
}