using System;
using System.Runtime.Serialization;

namespace Virtual.SmartCard.TLV.Simple
{
    public class BERFormatException : Exception
    {
        public BERFormatException()
        {
        }

        public BERFormatException(string message)
            : base(message)
        {
        }

        public BERFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BERFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}