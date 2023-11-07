using System;
using System.Collections.Generic;
using System.Linq;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.Parsers
{
    public class BinaryParser : IParser
    {
        #region IParser Members

        public byte ParseToByte(string input)
        {
            var bytes = Parse(input);
            CheckResultLength(bytes, sizeof(byte));

            return bytes[0];
        }

        public bool ParseToBool(string input)
        {
            var bytes = Parse(input);
            CheckResultLength(bytes, sizeof(bool));

            Guard.Against(bytes[0] > 1, "Boolean can only be 0 or 1!");

            return bytes[0] > 0;
        }

        public short ParseToInt16(string input)
        {
            var bytes = Parse(input);
            CheckResultLength(bytes, sizeof(short));

            short byteMsb = bytes.Length > 1 ? bytes[1] : (short)0x00;
            short byteLsb = bytes[0];

            short result = byteLsb;
            result |= (short)(byteMsb << Constants.BITS_PER_BYTE);

            return result;
        }

        public int ParseToInt32(string input)
        {
            var bytes = Parse(input);
            CheckResultLength(bytes, sizeof(int));

            int w1Msb = bytes.Length > 3 ? bytes[3] : 0x00;
            int w1Lsb = bytes.Length > 2 ? bytes[2] : 0x00;
            int w0Msb = bytes.Length > 1 ? bytes[1] : 0x00;
            int w0Lsb = bytes[0];

            int result = w0Lsb;
            result |= (w0Msb << Constants.BITS_PER_BYTE);
            result |= (w1Lsb << Constants.BITS_PER_BYTE * 2);
            result |= (w1Msb << Constants.BITS_PER_BYTE * 3);

            return result;
        }

        public long ParseToInt64(string input)
        {
            var bytes = Parse(input);
            CheckResultLength(bytes, sizeof(long));

            long w3Msb = bytes.Length > 7 ? bytes[7] : 0x00;
            long w3Lsb = bytes.Length > 6 ? bytes[6] : 0x00;
            long w2Msb = bytes.Length > 5 ? bytes[5] : 0x00;
            long w2Lsb = bytes.Length > 4 ? bytes[4] : 0x00;
            long w1Msb = bytes.Length > 3 ? bytes[3] : 0x00;
            long w1Lsb = bytes.Length > 2 ? bytes[2] : 0x00;
            long w0Msb = bytes.Length > 1 ? bytes[1] : 0x00;
            long w0Lsb = bytes[0];

            long result = w0Lsb;
            result |= (w0Msb << Constants.BITS_PER_BYTE);
            result |= (w1Lsb << Constants.BITS_PER_BYTE * 2);
            result |= (w1Msb << Constants.BITS_PER_BYTE * 3);
            result |= (w2Lsb << Constants.BITS_PER_BYTE * 4);
            result |= (w2Msb << Constants.BITS_PER_BYTE * 5);
            result |= (w3Lsb << Constants.BITS_PER_BYTE * 6);
            result |= (w3Msb << Constants.BITS_PER_BYTE * 7);

            return result;
        }

        #endregion


        private static void CheckResultLength(byte[] bytes, int typeSize)
        {
            Guard.Against(bytes == null, "You must have, at least, 1 byte!");
            Guard.Against(bytes.Length > typeSize,
                          String.Format("Size of must be, at most, {0} bytes!", typeSize));
        }


        public byte[] Parse(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            var reversedString = input.Reverse().ToArray();

            IList<byte> result = new List<byte>();

            int charCount = 0;

            byte _byte = 0x00;
            foreach (var @char in reversedString)
            {
                var quotient = charCount % Constants.BITS_PER_BYTE;

                if (charCount != 0 && quotient == 0)
                {
                    result.Add(_byte);
                    _byte = 0x00;
                }

                var value = Char.GetNumericValue(@char);
                Guard.Against(value < 0.0, String.Format("Invalid character '{0}' in binary input", @char));

                _byte |= (byte)((((byte)value) & 0x01) << quotient);

                charCount++;
            }
            result.Add(_byte);

            return result.ToArray();
        }
    }
}