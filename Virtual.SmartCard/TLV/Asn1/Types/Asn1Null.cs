using System.IO;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1Null : IAsn1Type<object>
    {
        public Asn1Null()
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Null);
            Length = new Asn1Length(Asn1LengthForm.Short, 0);
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public object Value { get { return null; } }

        public void Encode(Stream output)
        {
        }

        public void Decode(Stream input)
        {
        }
    }
}