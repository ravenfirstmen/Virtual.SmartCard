using System.IO;

namespace Virtual.SmartCards.Asn1.Tests.Utils
{
    public class ByteArrayStream : MemoryStream
    {
        public ByteArrayStream()
        {
        }

        public ByteArrayStream(int capacity) : base(capacity)
        {
        }

        public ByteArrayStream(byte[] buffer) : base(buffer)
        {
        }

        public ByteArrayStream(byte[] buffer, bool writable) : base(buffer, writable)
        {
        }

        public ByteArrayStream(byte[] buffer, int index, int count) : base(buffer, index, count)
        {
        }

        public ByteArrayStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable)
        {
        }

        public ByteArrayStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible) : base(buffer, index, count, writable, publiclyVisible)
        {
        }

        public override byte[] ToArray()
        {
            return ReverseBytes(base.ToArray());
        }

        private byte[] ReverseBytes(byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            int highBound = input.Length - 1;

            for (int i = 0; i < input.Length/2; i++)
            {
                (input[i], input[highBound]) = (input[highBound], input[i]);
                highBound--;
            }

            return input;
        }
    }
}