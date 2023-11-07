using System.IO;

namespace Virtual.SmartCard.TLV.Asn1.Types
{
    public interface IAsn1Type<T>
    {
        // metadata
        Asn1Tag Tag { get; }
        Asn1Length Length { get; }

        T Value { get; }

        void Encode(Stream output);
        void Decode(Stream input);

        string ToString();
    }
}