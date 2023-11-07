using System;
using System.IO;

namespace Virtual.SmartCard.Serializers
{
    public interface IByteSerializer
    {
        byte[] Serialize(bool value);
        byte[] Serialize(byte value);
        byte[] Serialize(sbyte value);
        byte[] Serialize(byte[] buffer);
        byte[] Serialize(byte[] buffer, int index, int count);
        byte[] Serialize(char ch);
        byte[] Serialize(char[] chars);
        byte[] Serialize(char[] chars, int index, int count);
        byte[] Serialize(double value);
        byte[] Serialize(decimal value);
        byte[] Serialize(short value);
        byte[] Serialize(ushort value);
        byte[] Serialize(int value);
        byte[] Serialize(uint value);
        byte[] Serialize(long value);
        byte[] Serialize(ulong value);
        byte[] Serialize(float value);
        byte[] Serialize(string value);
    }

    public class ByteSerializer : IByteSerializer
    {
        public byte[] Serialize(bool value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(byte value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(sbyte value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(byte[] buffer)
        {
            return buffer;
        }

        public byte[] Serialize(byte[] buffer, int index, int count)
        {
            return DoSerialize(w => w.Write(buffer, index, count));
        }

        public byte[] Serialize(char ch)
        {
            return DoSerialize(w => w.Write(ch));
        }

        public byte[] Serialize(char[] chars)
        {
            return DoSerialize(w => w.Write(chars));
        }

        public byte[] Serialize(char[] chars, int index, int count)
        {
            return DoSerialize(w => w.Write(chars, index, count));
        }

        public byte[] Serialize(double value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(decimal value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(short value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(ushort value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(int value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(uint value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(long value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(ulong value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(float value)
        {
            return DoSerialize(w => w.Write(value));
        }

        public byte[] Serialize(string value)
        {
            return DoSerialize(w => w.Write(value));
        }

        private byte[] DoSerialize(Action<BinaryWriter> @delegate)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    @delegate(writer);
                }

                return ms.ToArray();
            }
        }
    }
}