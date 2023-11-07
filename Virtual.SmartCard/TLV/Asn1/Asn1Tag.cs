using System.IO;
using Virtual.SmartCard.TLV.Asn1.Utils;

namespace Virtual.SmartCard.TLV.Asn1
{
    public class Asn1Tag
    {
        public Asn1Tag(Asn1Class @class, Asn1EncodingForm encodingForm, Asn1Type type)
            : this(@class, encodingForm, type, (ulong)type)
        {
        }

        public Asn1Tag(Asn1Class @class, Asn1EncodingForm encodingForm, Asn1Type type, ulong tagNumAsn1)
        {
            Class = @class;
            EncodingForm = encodingForm;
            Type = type;
            TagNumAsn1 = tagNumAsn1;
        }

        public Asn1Class Class { get; private set; }
        public Asn1EncodingForm EncodingForm { get; private set; }
        public Asn1Type Type { get; private set; }
        public ulong TagNumAsn1 { get; private set; }


        public void Encode(Stream output)
        {

            if (TagNumAsn1 < (int)Asn1Type.LongForm /* 31 */)
            {
                var outputByte = (byte) (
                                              ((byte)@Class << 6)
                                            | ((byte)EncodingForm << 5)
                                            | ((byte)TagNumAsn1 & Masks.TYPE_MASK)
                                        );
                output.WriteByte(outputByte);
            }
            else
            {
                int numAsn1BytesForEncoding = Asn1Utils.MinBytesNeededForEncoding(TagNumAsn1);

                var outputBytes = new byte[numAsn1BytesForEncoding + 1]; // + 1 para class e encoding form
                outputBytes[0] = (byte)(
                                              ((byte)@Class << 6)
                                            | ((byte)EncodingForm  << 5)
                                            | 0x1F // 31 = 00011111
                                        );

                for (int i = 1; i <= (numAsn1BytesForEncoding - 1); i++)
                {
                    outputBytes[i] = (byte)(((TagNumAsn1 >> (7 * (numAsn1BytesForEncoding - i))) & 0xFF) | (uint)Masks.HAS_TAG_NUMAsn1);
                }

                outputBytes[numAsn1BytesForEncoding] = (byte)((int)TagNumAsn1 & Masks.TAG_NUMAsn1_MASK); // ultimo byte tem que ter 8º bit a zero.

                WriteTo(output, outputBytes);
            } 
        }

        private void WriteTo(Stream output, byte[] outputBytes)
        {
            if (outputBytes == null)
            {
                return;
            }

            foreach (var outputByte in outputBytes)
            {
                output.WriteByte(outputByte);
            }
        }

        public static Asn1Tag Decode(Stream input)
        {
            int idOctet = input.ReadByte();
            if (idOctet == -1)
            {
                throw new Asn1FormatException("Class invalid format");
            }
            // class
            var @class = (Asn1Class)((idOctet & Masks.CLASS_MASK) >> 6);
            // encodingForm
            var encodingForm = (Asn1EncodingForm)((idOctet & Masks.ENCODING_FORM_MASK) >> 5);
            // type
            var type = (Asn1Type)(idOctet & Masks.TYPE_MASK);

            if (@class == Asn1Class.Universal && type == Asn1Type.LongForm)
            {
                throw new Asn1FormatException("TAG class universal cannot have a type of long form");
            }

            // tagNumAsn1
            var tagNumAsn1 = (ulong)type;

            if (type == Asn1Type.LongForm)
            {
                tagNumAsn1 = 0;

                var numAsn1OfBytes = 0;
                // tag numAsn1 muti byte
                byte bp;
                bool firstSubsequentByte = true;
                do
                {
                    var _byte = input.ReadByte();
                    if (_byte == -1)
                    {
                        throw new Asn1FormatException("Class invalid format");
                    }
                    numAsn1OfBytes++;
                    if (numAsn1OfBytes > 9) // max para ulong = 64bits / 7bits encoded
                    {
                        throw new Asn1FormatException("Tag numAsn1 cannot fit on ulong");
                    }
                    bp = (byte)_byte;
                    if (firstSubsequentByte)
                    {
                        if ((bp & Masks.TAG_NUMAsn1_MASK /*bit 7..1*/) == 0x00)
                        {
                            throw new Asn1FormatException("First subsequent shall not be 0");
                        }
                        firstSubsequentByte = false;
                    }
                    tagNumAsn1 <<= 7; // shift 7 para esquerda para os novos "7" bits
                    tagNumAsn1 += (uint)(bp & Masks.TAG_NUMAsn1_MASK);
                } while ((bp & Masks.HAS_TAG_NUMAsn1) == Masks.HAS_TAG_NUMAsn1 /* enquanto byte tiver "1" no bit + significativo*/);
            }

            return new Asn1Tag(@class, encodingForm, type, tagNumAsn1);
        }
    }
}