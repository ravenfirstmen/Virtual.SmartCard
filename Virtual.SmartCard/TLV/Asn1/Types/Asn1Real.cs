using System;
using System.IO;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public class Asn1Real : IAsn1Type<double>
    {
        public Asn1Real(Asn1Length length)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Integer);
            Length = length;
        }

        public Asn1Real(double value)
        {
            Tag = new Asn1Tag(Asn1Class.Universal, Asn1EncodingForm.Primitive, Asn1Type.Integer);
            Value = value;
        }

        public Asn1Tag Tag { get; private set; }
        public Asn1Length Length { get; private set; }
        public double Value { get; private set; }

        public void Encode(Stream output)
        {
            throw new System.NotImplementedException();
        }

        public void Decode(Stream input)
        {
            Guard.Against(Length == null, "Length is not defined!");

            var contentsLength = (int)Length.Value;

            if (contentsLength == 0)
            {
                Value = 0.0;

                return;
            }

            var bytes = new byte[contentsLength];
            if (input.Read(bytes, 0, contentsLength) < contentsLength)
            {
                throw new Asn1FormatException("Error Decoding Asn1Real");
            }

            byte informationOctet = bytes[0];

            // special real
            if ((informationOctet & Masks.REAL_SPECIAL_VALUE) == Masks.REAL_SPECIAL_VALUE)
            {
                DecodeSpecialRealValue(informationOctet);

                return;
            }

            // base 10 -> caracter encoding
            if ((informationOctet & Masks.REAL_BASE_10_ENCODED) == Masks.REAL_BASE_10_ENCODED)
            {
                DecodeBase10(bytes);

                return;
            }

            // bases 2, 4 e 8 -> binary encoding

            // sinal
            double sign = (informationOctet & Masks.REAL_SIGN_MASK) == Masks.REAL_SIGN_MASK ? -1.0 : 1.0;

            // base
            int @base = 2;
            switch (informationOctet & Masks.REAL_BASE_MASK)
            {
                case Masks.REAL_BASE_2:
                    @base = 2;
                    break;
                case Masks.REAL_BASE_4:
                    @base = 8;
                    break;
                case Masks.REAL_BASE_16:
                    @base = 16;
                    break;
                default:
                    throw new Asn1FormatException("Error Decoding Asn1Real");
            }

            // fator escala
            int scalingFactor = (informationOctet & Masks.REAL_SCALING_FACTOR_MASK) >> 2;

            // exponent
            int exponentEncodedLength = (informationOctet & Masks.REAL_EXPONENT_MASK) + 1;
            int valueNIndex = exponentEncodedLength + 1;
            int exponentIndex = 1;
            if ((informationOctet & Masks.REAL_EXPONENT_MASK) == Masks.REAL_EXPONENT_NEXT_OCTET)
            {
                exponentEncodedLength = bytes[1];
                valueNIndex++;
                exponentIndex++;
            }

            long exponentValue = -1;
            for (int i = 0; i < exponentEncodedLength; i++)
            {
                var exponentByte = bytes[i + exponentIndex];
                int numShiftBits = 8 * (exponentEncodedLength - i - 1);
                exponentValue &= unchecked (((exponentByte << numShiftBits) | ~(0xFF << numShiftBits)));
            }

            ulong valueN = 0;
            for (int i = 0; i < contentsLength - valueNIndex; i++)
            {
                valueN |= (ulong)bytes[i + valueNIndex] << (8 * (contentsLength - valueNIndex - i - 1));
            }

            double mantissa = sign*valueN*Math.Pow(2, scalingFactor); 

            Value = mantissa * Math.Pow((double)@base, (double)exponentValue);

        }


        private void DecodeBase10(byte[] bytes)
        {
            //switch (bytes[0] & 0x3F)
            //{
            //    case 0x01:
            //        NR_form = 1;
            //        break;
            //    case 0x02:
            //        NR_form = 2;
            //        break;
            //    case 0x03:
            //        NR_form = 3;
            //        break;
            //    default:
            //        NR_form = 0xFF; // impossible case;
            //        sprintf_s(error, 255, "Wrong NR form");
            //        return In();
            //}           
        }

        private void DecodeSpecialRealValue(byte octet)
        {
            switch (octet)
            {
                case Masks.REAL_PLUS_INFINITY:
                    Value = Double.PositiveInfinity;
                    break;
                case Masks.REAL_MINUS_INFINITY:
                    Value = Double.NegativeInfinity;
                    break;
                case Masks.REAL_NOT_NUMAsn1:
                    Value = Double.NaN;
                    break;
                default:
                    throw new Asn1FormatException("Asn1Real: invalid real encoding");
            }
        }
    }
}