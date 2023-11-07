using System.IO;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Simple;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: BER :: LENGTH :: Decoding")]
    public class BerLengthDecodingTests
    {
        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_Empty_Stream()
        {
            using (var ms = new MemoryStream())
            {
                BERLength.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_First_Octet_All_1s()
        {
            var b = new byte[] { (byte)(Masks.LENGTH_LONG_FORM_MASK | Masks.LENGTH_MASK) };
            using (var ms = new MemoryStream(b))
            {
                BERLength.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_Length_Value_Bigger_ulong()
        {
            var b = new byte[] { (byte)(Masks.LENGTH_LONG_FORM_MASK | (sizeof(ulong) + 1)) };
            using (var ms = new MemoryStream(b))
            {
                BERLength.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_Wrong_Encoded_Bytes()
        {
            var b = new byte[] { 
                (byte)(Masks.LENGTH_LONG_FORM_MASK | (sizeof(ulong) - 1)), // espeados 7 bytes
                0x00,
                0x00
            };
            using (var ms = new MemoryStream(b))
            {
                BERLength.Decode(ms);
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
                var length = BERLength.Decode(ms);

                Assert.AreEqual(BERLengthForm.Short, length.Form);
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
                var length = BERLength.Decode(ms);

                Assert.AreEqual(BERLengthForm.Short, length.Form);
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
                var length = BERLength.Decode(ms);

                Assert.AreEqual(BERLengthForm.Long, length.Form);
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
                var length = BERLength.Decode(ms);

                Assert.AreEqual(BERLengthForm.Long, length.Form);
                Assert.AreEqual(1024, length.Value);
            }

        }
    }
}
