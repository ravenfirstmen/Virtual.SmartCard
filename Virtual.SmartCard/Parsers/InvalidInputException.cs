using System;

namespace Virtual.SmartCard.Parsers
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message)
            : base(message)
        {
        }
    }
}