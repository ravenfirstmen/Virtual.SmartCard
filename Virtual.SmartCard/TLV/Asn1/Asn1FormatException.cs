using System;
using System.Runtime.Serialization;

namespace Virtual.SmartCard.TLV.Asn1
{
    public class Asn1FormatException : Exception
    {
        public Asn1FormatException()
        {
        }

        public Asn1FormatException(string message)
            : base(message)
        {
        }

        public Asn1FormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected Asn1FormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}