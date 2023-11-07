using System.IO;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Asn1.Types;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: Bitstring")]
    public class Asn1BitstringTests
    {
        [TestFixture]
        public class Asn1BitstringEncodingTests
        {
            [Test]
            public void EncodingTest1()
            {
                using (var ms = new MemoryStream())
                {
                    var bitstring = new Asn1Bitstring(new byte[] {0x0A, 0x3B, 0x5F, 0x29, 0x1C, 0xD});
                    bitstring.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x04, 0x0A, 0x3B, 0x5F, 0x29, 0x1C, 0xD0 }, ms.ToArray()));
                    Assert.AreEqual(4, bitstring.UnusedBits);
                    Assert.AreEqual(Asn1LengthForm.Short, bitstring.Length.Form);
                }
            }

            [Test]
            public void Encoding_Empty_When_Empty_Input()
            {
                using (var ms = new MemoryStream())
                {
                    var bitstring = new Asn1Bitstring(new byte[] { });
                    bitstring.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x00 }, ms.ToArray()));
                    Assert.AreEqual(0, bitstring.UnusedBits);
                    Assert.AreEqual(Asn1LengthForm.Short, bitstring.Length.Form);
                }
            }

            [Test]
            public void Encoding_Empty_When_Null_Input()
            {
                using (var ms = new MemoryStream())
                {
                    var bitstring = new Asn1Bitstring((byte[])null);
                    bitstring.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x00 }, ms.ToArray()));
                    Assert.AreEqual(0, bitstring.UnusedBits);
                    Assert.AreEqual(Asn1LengthForm.Short, bitstring.Length.Form);
                }
            }
        }

        [TestFixture]
        public class Asn1BitstringDecodingTests
        {
            [Test]
            public void DecodingTest1()
            {
                var b = new byte[] { 0x04, 0x0A, 0x3B, 0x5F, 0x29, 0x1C, 0xD0 };

                using (var ms = new ByteArrayStream(b))
                {
                    var bitstring = new Asn1Bitstring(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    bitstring.Decode(ms);

                    Assert.AreEqual(true,
                                    ByteArrayUtils.AreEqual(bitstring.Value,
                                                            new byte[] {0x0A, 0x3B, 0x5F, 0x29, 0x1C, 0xD}));
                }
            }

            [Test]
            public void Decoding_Value_Null_When_Empty_Input()
            {
                var b = new byte[] { };

                using (var ms = new ByteArrayStream(b))
                {
                    var bitstring = new Asn1Bitstring(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    bitstring.Decode(ms);

                    Assert.AreEqual(null, bitstring.Value);
                }               
            }

        }
    }
}