using System.IO;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1OctetString : IAsn1Type<byte[]>
    {
        public Asn1OctetString()
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.OctetString);
            Length = new Asn1Length(Asn1LengthForm.Short, 1);
        }

        public Asn1OctetString(byte[] value)
            : this()
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.OctetString);
            Length = new Asn1Length(Asn1LengthForm.Short, value != null ? (ulong)value.Length : 0);
            Value = value;
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public byte[] Value { get; private set; }
        
        public void Encode(Stream output)
        {
            output.Write(Value, 0, Value.Length);
        }

        public void Decode(Stream input)
        {
            Guard.Against(Length == null, "Length is not defined!");

            if (Length.Form == Asn1LengthForm.Indefinite)
            {
                // PARA REVER .......
                var contentsLength = (int)Length.Value;
                Value = new byte[contentsLength];
                if (input.Read(Value, 0, contentsLength) < contentsLength)
                {
                    throw new Asn1FormatException("Error decoding Asn1OctetString");
                }
            }
            else
            {
                var contentsLength = (int)Length.Value;
                Value = new byte[contentsLength];
                if (input.Read(Value, 0, contentsLength) < contentsLength)
                {
                    throw new Asn1FormatException("Error decoding Asn1OctetString");
                }
            }
        }
    }
}