using System.IO;
using Virtual.SmartCard.Infrastructure;
using Virtual.SmartCard.TLV.Asn1.Utils;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1Integer : IAsn1Type<long>
    {
        public Asn1Integer(Asn1Length length)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Integer);
            Length = length;
        }

        public Asn1Integer(long value)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Integer);
            Value = value;
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }

        public long Value { get; private set; }

        public void Encode(Stream output)
        {
            int numAsn1BytesEncoding = Asn1Utils.MinBytesNeededForEncoding(Value);

            for (int i = 0; i < numAsn1BytesEncoding; i++)
            {
                byte outputByte = (byte) ((Value >> 8*(i)) & 0xFF);
                output.WriteByte(outputByte);
            }

            Length = new Asn1Length(numAsn1BytesEncoding > 1 ? Asn1LengthForm.Long : Asn1LengthForm.Short, (ulong)numAsn1BytesEncoding);
        }

        public void Decode(Stream input)
        {
            Guard.Against(Length == null, "Length is not defined!");

            var contentsLength = (int) Length.Value;

            var bytes = new byte[contentsLength];
            if (input.Read(bytes, 0, contentsLength) < contentsLength)
            {
                throw new Asn1FormatException("Error decoding Asn1Integer");
            }

            if ((bytes[0] & Masks.TWO_COMPLEMENTS_MASK) == Masks.TWO_COMPLEMENTS_MASK) // em 2 complemento
            {
                Value = -1;
                for (int i = 0; i < contentsLength; i++)
                {
                    int numShiftBits = 8*(contentsLength - i - 1);
                    Value &= (((bytes[i]) << numShiftBits) | ~(0xFF << numShiftBits));
                }
            }
            else
            {
                Value = 0;
                for (int i = 0; i < contentsLength; i++)
                {
                    Value |= (long)(bytes[i] & 0xFF) << (8*(contentsLength - i - 1));
                }
            }
        }
    }
}