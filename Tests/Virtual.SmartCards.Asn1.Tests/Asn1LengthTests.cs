using System.IO;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: Length")]
    public class Asn1LengthTests
    {
        #region Decoding

        [TestFixture]
        public class Asn1LengthDecodingTests : Asn1LengthTests
        {
            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_Empty_Stream()
            {
                using (var ms = new MemoryStream())
                {
                    Asn1Length.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_First_Octet_All_1s()
            {
                var b = new byte[] { (byte)(Masks.LENGTH_LONG_FORM_MASK | Masks.LENGTH_MASK) };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Length.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_Length_Value_Bigger_ulong()
            {
                var b = new byte[] { (byte)(Masks.LENGTH_LONG_FORM_MASK | (sizeof(ulong) + 1)) };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Length.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_Wrong_Encoded_Bytes()
            {
                var b = new byte[] { 
                (byte)(Masks.LENGTH_LONG_FORM_MASK | (sizeof(ulong) - 1)), // espeados 7 bytes
                0x00,
                0x00
            };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Length.Decode(ms);
                }

            }

            [Test]
            public void Decoding_Test1()
            {
                var b = new byte[] { 
                (byte)(10 & Masks.LENGTH_MASK)
            };
                using (var ms = new MemoryStream(b))
                {
                    var length = Asn1Length.Decode(ms);

                    Assert.AreEqual(Asn1LengthForm.Short, length.Form);
                    Assert.AreEqual(10, length.Value);
                }

            }

            [Test]
            public void Decoding_Test2()
            {
                var b = new byte[] { 
                (byte)(38 & Masks.LENGTH_MASK),
            };
                using (var ms = new MemoryStream(b))
                {
                    var length = Asn1Length.Decode(ms);

                    Assert.AreEqual(Asn1LengthForm.Short, length.Form);
                    Assert.AreEqual(38, length.Value);
                }

            }

            [Test]
            public void Decoding_Test3()
            {
                var b = new byte[] { 
                (byte)(Masks.LENGTH_LONG_FORM_MASK | (2 & Masks.LENGTH_MASK)),
                0x01,
                0xB3
            };
                using (var ms = new MemoryStream(b))
                {
                    var length = Asn1Length.Decode(ms);

                    Assert.AreEqual(Asn1LengthForm.Long, length.Form);
                    Assert.AreEqual(435, length.Value);
                }

            }

            [Test]
            public void Decoding_Test4()
            {
                var b = new byte[] { 
                (byte)(Masks.LENGTH_LONG_FORM_MASK | (2 & Masks.LENGTH_MASK)),
                0x04,
                0x00
            };
                using (var ms = new MemoryStream(b))
                {
                    var length = Asn1Length.Decode(ms);

                    Assert.AreEqual(Asn1LengthForm.Long, length.Form);
                    Assert.AreEqual(1024, length.Value);
                }

            }

        }

        #endregion

        #region Encoding

        [TestFixture]
        public class Asn1LengthEncodingTests : Asn1LengthTests
        {

            [Test]
            public void Encode_Test1()
            {
                var length = new Asn1Length(Asn1LengthForm.Short, 0x10);

                using (var ms = new MemoryStream())
                {
                    length.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x10 }, ms.ToArray()));
                }
            }

            [Test]
            public void Encode_Test2()
            {
                var length = new Asn1Length(Asn1LengthForm.Short, 0x7F);

                using (var ms = new MemoryStream())
                {
                    length.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x7F }, ms.ToArray()));
                }
            }

            [Test]
            public void Encode_Test3()
            {
                var referenceBytes = new byte[]
                                         {
                                             (byte) (Masks.LENGTH_LONG_FORM_MASK | (2 & Masks.LENGTH_MASK)),
                                             0x01,
                                             0xB3
                                         };

                var length = new Asn1Length(Asn1LengthForm.Long, 435);

                using (var ms = new ByteArrayStream())
                {
                    length.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(referenceBytes, ms.ToArray()));
                }
            }

            [Test]
            public void Encode_Test4()
            {
                var length = new Asn1Length(Asn1LengthForm.Long, 1024);

                using (var ms = new ByteArrayStream())
                {
                    length.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x82, 0x04, 0x00 }, ms.ToArray()));
                }
            }
        }

        #endregion
    }
}