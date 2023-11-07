using System.IO;
using Virtual.SmartCard.TLV.Asn1.Utils;

namespace Virtual.SmartCard.TLV.Asn1
{
    public class Asn1Length
    {
        public Asn1Length(Asn1LengthForm form, ulong value)
        {
            Form = form;
            Value = value;
        }

        public Asn1LengthForm Form { get; private set; }
        public ulong Value { get; private set; }

        public void Encode(Stream output)
        {
            if (Value <= 0x7F) // apenas 7 bits disponiveis para valor
            {
                var outputByte = (byte) ((int) Value & Masks.LENGTH_MASK);
                output.WriteByte(outputByte);
            }
            else
            {
                int minBytesNeededForEncoding = Asn1Utils.MinBytesNeededForEncoding(Value);

                for (int i = 0; i < minBytesNeededForEncoding; i++)
                {
                    output.WriteByte((byte)((Value >> 8 * i) & 0xFF));
                }

                output.WriteByte((byte)(Masks.LENGTH_LONG_FORM_MASK | minBytesNeededForEncoding));
            }
        }


        public static Asn1Length Decode(Stream input)
        {

            int lengthOctet = input.ReadByte();
            if (lengthOctet == -1)
            {
                throw new Asn1FormatException("Invalid length.");
            }

            // form
            var lengthForm = Asn1LengthForm.Short;
            var lengthValue = (ulong)(lengthOctet & Masks.LENGTH_MASK);

            var b8Value = ((lengthOctet & Masks.LENGTH_LONG_FORM_MASK) >> 7);
            if (b8Value == 1 && lengthValue == 0)
            {
                lengthForm = Asn1LengthForm.Indefinite;
            }
            if (b8Value == 1 && lengthValue != 0)
            {
                lengthForm = Asn1LengthForm.Long;
            }

            if (lengthForm == Asn1LengthForm.Long)
            {
                if ((lengthOctet & Masks.LENGTH_MASK) == Masks.LENGTH_MASK)
                {
                    throw new Asn1FormatException("Invalid long form length. First octet shall not be 1's");
                }

                // neste caso lengthValue = nº de bytes para length
                if (lengthValue > sizeof(ulong))
                {
                    throw new Asn1FormatException("LengthForm cannot fit in a ulong value");
                }

                var bytes = new byte[lengthValue];
                if (input.Read(bytes, 0, (int)lengthValue) < (int)lengthValue)
                {
                    throw new Asn1FormatException("Error decoding length");
                }

                var nBytes = (int)lengthValue;
                lengthValue = 0;
                for (int i = 0; i < nBytes; i++)
                {
                    lengthValue |= ( (ulong)(bytes[i] & 0xFF) << (8 * (nBytes - i - 1)) );
                }
            }

            return new Asn1Length(lengthForm, lengthValue);
        }
    }
}