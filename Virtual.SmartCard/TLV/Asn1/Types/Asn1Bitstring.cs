using System;
using System.IO;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1Bitstring : IAsn1Type<byte[]>
    {
        public Asn1Bitstring(Asn1Length length)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.BitString);
            Length = length;
        }

        public Asn1Bitstring(byte[] value)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.BitString);
            Length = new Asn1Length(Asn1LengthForm.Short, value != null ? (ulong)value.Length : 0);
            if (value != null && value.Length > 0)
            {
                UnusedBits = CalculateUnusedBits(value[0]);
                AllocateAndCopyToValue(value, 0);
            }
            else
            {
                UnusedBits = 0;
                Value = null;
            }
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public uint UnusedBits { get; private set; }
        public byte[] Value { get; private set; }

        public void Encode(Stream output)
        {
            if (Value != null && Value.Length > 0)
            {
                output.WriteByte((byte) UnusedBits);
                output.Write(Value, 0, (int) Length.Value - 1);
                output.WriteByte((byte) (Value[Value.Length - 1] << (int) UnusedBits));
            }
            else
            {
                output.WriteByte(0x00);
            }
        }

        public void Decode(Stream input)
        {
            Guard.Against(Length == null, "Length is not defined!");

            var contentsLength = (int)Length.Value;

            if (contentsLength > 0)
            {
                var bytes = new byte[contentsLength];
                if (input.Read(bytes, 0, contentsLength) < contentsLength)
                {
                    throw new Asn1FormatException("Error decoding Asn1Bitstring");
                }

                UnusedBits = (uint)bytes[0];
                if (UnusedBits > 7)
                {
                    throw new Asn1FormatException("Error decoding Asn1Bitstring. Unused bits between 0 and 7.");
                }
                AllocateAndCopyToValue(bytes, 1);
                SetLastByte();
            }
        }


        private void AllocateAndCopyToValue(byte[] source, int offset)
        {
            if (source.Length > 0)
            {
                Value = new byte[source.Length - offset];
                Buffer.BlockCopy(source, offset, Value, 0, source.Length - offset);
            }
        }

        private void SetLastByte()
        {
            if (UnusedBits > 0)
            {
                Value[Value.Length - 1] >>= (int)UnusedBits;
            }
        }

        private uint CalculateUnusedBits(byte b)
        {
            uint unused = 0;
            for (int i = sizeof(byte) * 8 - 1; i >= 0; i--, unused++)
            {
                if (((b >> i) & 0x01) == 0x01)
                {
                    break;
                }
            }

            return unused;
        }
     }
}