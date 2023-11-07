using System.Collections.Generic;
using System.IO;
using System.Text;
using Virtual.SmartCard.Infrastructure;
using Virtual.SmartCard.TLV.Asn1.Utils;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1ObjectIdentifier : IAsn1Type<uint[]>
    {
        public Asn1ObjectIdentifier(Asn1Length length)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.ObjectIdentifier);
            Length = length;
        }

        public Asn1ObjectIdentifier(uint[] value)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.ObjectIdentifier);
            Length = new Asn1Length(Asn1LengthForm.Short, value != null ? (ulong)value.Length : 0);
            Value = value;
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public uint[] Value { get; private set; }

        public void Encode(Stream output)
        {
            // regra SID1*40 + SID2 = valor subidentifier

            uint firstSubidentifier = 40 * Value[0] + Value[1];

            int totalLength = 0;

            for (int i = (Value.Length - 1); i > 0; i--)
            {
                uint subidentifier = i == 1 ? firstSubidentifier : Value[i];

                output.WriteByte((byte)(subidentifier & Masks.OID_NUMAsn1_MASK));

                int numAsn1BytesEncoding = Asn1Utils.MinBytesNeededForEncoding(subidentifier);

                for (int j = 1; j <= (numAsn1BytesEncoding - 1); j++)
                {
                    output.WriteByte((byte)(((subidentifier >> (7 * j)) & Masks.BYTE_MASK) | Masks.OID_HAS_MORE));
                }

                totalLength += numAsn1BytesEncoding;
            }

            Length = new Asn1Length(totalLength > 1 ? Asn1LengthForm.Long : Asn1LengthForm.Short, (ulong)totalLength);
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

            var identifiers = new List<uint>();

            int globalIndex = 0;
            while (globalIndex < contentsLength)
            {
                identifiers.Add(DecodeSubIdentifier(bytes, ref globalIndex));
                globalIndex++;
            }

            Normalize(identifiers);
        }

        private uint DecodeSubIdentifier(byte[] bytes, ref int globalIndex)
        {
            uint result = 0;

            int length = 0;
            for (int i = globalIndex; (bytes[i] & Masks.OID_HAS_MORE) == Masks.OID_HAS_MORE; i++, length++)
            {
            }

            for (int i = 0; i <= length; i++)
            {
                result |= (uint)((bytes[globalIndex + i] & Masks.OID_NUMAsn1_MASK) << ((length - i) * 7));
            }

            globalIndex += length;

            return result;
        }

        private void Normalize(IList<uint> subidentifiers)
        {
            Guard.Against(subidentifiers.Count == 0, "Error decoding Asn1ObjectIdentifier");

            // regra SID1*40 + SID2 = valor subidentifier
            // SID1 só pode ter os valores de 0, 1 e 2
            // então

            var normalized = new List<uint>();

            var subIdentifier1 = subidentifiers[0];
            if (subIdentifier1 < 40)
            {
                normalized.Add(0);
                normalized.Add(subIdentifier1);
            }
            else if (subIdentifier1 < 80)
            {
                normalized.Add(1);
                normalized.Add(subIdentifier1 - 40);
            }
            else 
            {
                normalized.Add(2);
                normalized.Add(subIdentifier1 - 80);
            }

            for (int i = 1; i < subidentifiers.Count; i++)
            {
                normalized.Add(subidentifiers[i]);
            }

            Value = normalized.ToArray();
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            bool first = true;
            foreach (var u in Value)
            {
                result.AppendFormat("{0}{1}", first ? string.Empty : ".", u);
                first = false;
            }

            return result.ToString();
        }
    }
}