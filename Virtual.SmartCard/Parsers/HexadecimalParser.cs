using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.Parsers
{
    public class HexadecimalParser : IParser
    {
        #region IParser Members

        public byte ParseToByte(string input)
        {
            CheckInput(input, sizeof(byte));
            return byte.Parse(input, NumberStyles.HexNumber);
        }

        public bool ParseToBool(string input)
        {
            CheckInput(input, sizeof(byte));
            var res = byte.Parse(input, NumberStyles.HexNumber);
            Guard.Against(res > 1, "Invalid boolean value!");

            return res > 0;
        }

        public short ParseToInt16(string input)
        {
            CheckInput(input, sizeof(short));
            return short.Parse(input, NumberStyles.HexNumber);
        }

        public int ParseToInt32(string input)
        {
            CheckInput(input, sizeof(int));
            return int.Parse(input, NumberStyles.HexNumber);
        }

        public long ParseToInt64(string input)
        {
            CheckInput(input, sizeof(long));
            return long.Parse(input, NumberStyles.HexNumber);
        }

        #endregion

        private static void CheckInput(string input, int typeSize)
        {
            Guard.Against(input == null, "Invalid input!");
            Guard.Against(input.Length > 2 * typeSize,
                          String.Format("Size of must be, at least, {0} characters!", typeSize));
        }

        public byte[] Parse(string input)
        {
            Guard.Against(String.IsNullOrEmpty(input), "Cannot parse empty string!");

            var byteLength = Math.Min(2, input.Length);
            var szByte = input.Substring(0, byteLength);
            var bytesToParse = input.Remove(0, byteLength);

            IList<byte> result = new List<byte>();
            while (!String.IsNullOrEmpty(szByte))
            {
                result.Add(ParseToByte(szByte));

                byteLength = Math.Min(2, bytesToParse.Length);
                szByte = bytesToParse.Substring(0, byteLength);
                bytesToParse = bytesToParse.Remove(0, byteLength);
            }

            return result.ToArray();
        }

    }
}