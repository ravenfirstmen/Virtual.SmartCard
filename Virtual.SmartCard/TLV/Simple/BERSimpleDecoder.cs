using System.Collections.Generic;
using System.IO;
using Virtual.SmartCard.Utils;

namespace Virtual.SmartCard.TLV.Simple
{
    public class BERSimpleDecoder
    {
        public IList<BERTLV> Parse(Stream contents)
        {
            if (contents.Length <= 1 || !contents.CanRead)
            {
                return null;
            }

            var result = new List<BERTLV>();

            do
            {
                var tag = DecodeTag(contents);
                var length = DecodeLength(contents);
                if (tag == null || length == null)
                {
                    break;
                }

                var tlv = new BERTLV
                    {
                        Tag = tag,
                        Length = length
                    };
                if (tlv.Tag.EncodingForm == BEREncodingForm.Constructed)
                {
                    tlv.AddChilds(Parse(contents));
                }
                else
                {
                    ReadTLVContents(tlv, contents);
                }

                result.Add(tlv);

            } while (true);


            return result;
        }

        // wikipedia: http://en.wikipedia.org/wiki/Basic_Encoding_Rules#BER_encoding
        private BERTAG DecodeTag(Stream contents)
        {
            int idOctet = contents.ReadByte();
            if (idOctet == -1)
            {
                return null;
            }
            // class
            var @class = (BERClass)((idOctet & 0xC0 /*11000000*/) >> 6);
            // encodingForm
            var encodingForm = (BEREncodingForm)((idOctet & 0x20 /*00100000*/) >> 5);
            // type
            var type = (BERType)(idOctet & 0x1F /*00011111*/);

            if (@class == BERClass.Universal && type == BERType.LongForm)
            {
                throw new BERFormatException("TAG class universal cannot have a type of long form");
            }

            // tagNumber
            uint tagNumber = 0;

            if (type == BERType.LongForm)
            {
                // tag number muti byte
                byte bp;
                bool firstSubsequent = true;
                do
                {
                    var _byte = contents.ReadByte();
                    if (_byte == -1)
                    {
                        throw new BERFormatException("Class invalid format");
                    }
                    bp = (byte)_byte;
                    if (firstSubsequent)
                    {
                        if ((bp & 0x7F /*bit 7..1*/) == 0x00)
                        {
                            throw new BERFormatException("First subsequent shall not be 0");
                        }
                        firstSubsequent = false;
                    }
                    tagNumber <<= 7; // shift 7 para esquerda para os novos "7" bits
                    tagNumber += (uint)(bp & 0x7F /*00011111*/);
                } while ((bp & 0x80 /*10000000*/) == 0x80 /*10000000 - enquanto byte tiver "1" no bit + significativo*/);
            }
            else
            {
                // tag de 1 byte
                tagNumber = (uint)idOctet & 0x1F /*00011111*/;
            }

            return new BERTAG(@class, encodingForm, type, tagNumber);
        }

        // wikipedia: http://en.wikipedia.org/wiki/Basic_Encoding_Rules#BER_encoding
        private BERLength DecodeLength(Stream contents)
        {
            int lengthOctet = contents.ReadByte();
            if (lengthOctet == -1)
            {
                return null;
            }
            // form
            var lengthForm = BERLengthForm.Short;
            var lengthValue = (uint)(lengthOctet & 0x7F) /*01111111*/;

            var b8Value = ((lengthOctet & 0x80 /*10000000*/) >> 7);
            if (b8Value == 1 && lengthValue == 0)
            {
                lengthForm = BERLengthForm.Indefinite;
            }
            if (b8Value == 1 && lengthValue != 0)
            {
                lengthForm = BERLengthForm.Long;
            }

            if (lengthForm == BERLengthForm.Long)
            {
                if ((lengthOctet & 0x7F) == 0x7F /*01111111*/)
                {
                    throw new BERFormatException("Invalid long form length. First octet shall not be 1's");
                }

                // neste caso lengthValue = nº de bytes para length
                if (lengthValue > sizeof(uint))
                {
                    throw new BERFormatException("Length cannot fit in a uint value");
                }

                var lv = lengthValue;
                var bytes = new byte[lv];
                int bc = 0;
                for (int p = 0; p < lv && bc != -1; p++)
                {
                    bc = contents.ReadByte();
                    if (bc != -1)
                    {
                        bytes[p] = (byte)bc;
                    }
                }
                lengthValue = (uint)BytesConverter.BytesToInt32(bytes);
            }

            return new BERLength(lengthForm, lengthValue);
        }

        private void ReadTLVContents(BERTLV tlv, Stream contents)
        {
            // bem... presume-se que length já vem preenchida
            if (tlv.Length.Form != BERLengthForm.Indefinite && tlv.Length.Value > 0)
            {
                tlv.Contents = new byte[tlv.Length.Value];
                contents.Read(tlv.Contents, 0, (int)tlv.Length.Value);
            }
            else
            {
                var toRead = contents.Length - contents.Position;
                if (toRead < 2) // pelo menos EOC
                {
                    throw new BERFormatException("Invalid contents");
                }
                tlv.Contents = new byte[toRead];
                contents.Read(tlv.Contents, 0, (int)toRead);
                var lasIndex = tlv.Contents.Length - 1;
                if ((tlv.Contents[lasIndex] != 0x00) && (tlv.Contents[lasIndex - 1] != 0x00))
                {
                    throw new BERFormatException("Invalid contents");
                }
            }
        }
    }
}