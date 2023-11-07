using System;

namespace Virtual.SmartCard.Formaters
{
    public class HexadecimalFormater : BaseFormater
    {
        public HexadecimalFormater(bool value) // bool
            : base(value)
        {
        }

        public HexadecimalFormater(byte value) // byte
            : base(value)
        {
        }

        public HexadecimalFormater(Int16 value) // short
            : base(value)
        {
        }

        public HexadecimalFormater(Int32 value) // int
            : base(value)
        {
        }

        public HexadecimalFormater(Int64 value) // long
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
                foreach (var @byte in bytes)
                {
                    Representation = String.Concat(Representation, String.Format("{0:X2}", @byte));
                }
            }
        }
    }
}