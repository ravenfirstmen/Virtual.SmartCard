using System;

namespace Virtual.SmartCard
{
    public class SmartCardException : Exception
    {
        public SmartCardException(Int32 errorCode)
            : base(SmartCardErrors.GetMessageFrom(errorCode))
        {
        }

        public SmartCardException(string message)
            : base(message)
        {
        }

        public SmartCardException(string message, Int32 errorCode)
            : base(String.Format("{0}{1}{2}", message, Environment.NewLine, SmartCardErrors.GetMessageFrom(errorCode)))
        {
        }
    }
}