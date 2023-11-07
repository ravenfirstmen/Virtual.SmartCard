using System;

namespace Virtual.SmartCard.Formaters
{
    public abstract class BaseFormater : IFormater
    {
        protected BaseFormater(bool value) // byte
        {
            NormalizeAndFormat(BitConverter.GetBytes(value));
        }

        protected BaseFormater(byte value) // byte
        {
            NormalizeAndFormat(new byte[] { value });
        }

        protected BaseFormater(Int16 value) // short
        {
            NormalizeAndFormat(BitConverter.GetBytes(value));
        }

        protected BaseFormater(Int32 value) // int
        {
            NormalizeAndFormat(BitConverter.GetBytes(value));
        }

        protected BaseFormater(Int64 value) // long
        {
            NormalizeAndFormat(BitConverter.GetBytes(value));
        }

        protected string Representation { get; set; }

        #region IFormater Members

        public override string ToString()
        {
            return Representation;
        }

        #endregion

        protected abstract void Format(byte[] bytes);

        private void NormalizeAndFormat(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            Format(bytes);
        }
    }
}