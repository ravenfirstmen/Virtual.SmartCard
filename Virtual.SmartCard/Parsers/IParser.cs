using System;

namespace Virtual.SmartCard.Parsers
{
    public interface IParser
    {
        byte[] Parse(string input);
        byte ParseToByte(string input);
        bool ParseToBool(string input);
        Int16 ParseToInt16(string input);
        Int32 ParseToInt32(string input);
        Int64 ParseToInt64(string input);
    }
}