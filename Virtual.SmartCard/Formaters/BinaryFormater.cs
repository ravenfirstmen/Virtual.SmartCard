using System;
using System.Linq;

namespace Virtual.SmartCard.Formaters
{
    public class BinaryFormater : BaseFormater
    {
        public BinaryFormater(bool value) // byte
            : base(value)
        {
        }

        public BinaryFormater(byte value) // byte
            : base(value)
        {
        }

        public BinaryFormater(Int16 value) // short
            : base(value)
        {
        }

        public BinaryFormater(Int32 value) // int
            : base(value)
        {
        }

        public BinaryFormater(Int64 value) // long
            : base(value)
        {
        }

        protected override void Format(byte[] bytes)
        {
            if (bytes == null)
            {
                Representation = String.Empty;
            }
            else
            {
                var reversedBytes = bytes.Reverse();
                foreach (var reversedByte in reversedBytes)
                {
                    for (int i = 0; i < Constants.BITS_PER_BYTE; i++)
                    {
                        Representation = String.Concat(reversedByte >> i & 0x01, Representation);
                    }
                }
            }
        }
    }
}