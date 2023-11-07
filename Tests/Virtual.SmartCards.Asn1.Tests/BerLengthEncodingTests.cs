using System.IO;
using NUnit.Framework;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: BER :: LENGTH :: Encoding")]
    public class BerLengthEncodingTests
    {

        [Test]
        public void Encode_Test1()
        {
            var length = new BERLength(BERLengthForm.Short, 0x10);

            using (var ms = new MemoryStream())
            {
                length.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x10 }, ms.ToArray()));
            }
        }

        [Test]
        public void Encode_Test2()
        {
            var length = new BERLength(BERLengthForm.Short, 0x7F);

            using (var ms = new MemoryStream())
            {
                length.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x7F }, ms.ToArray()));
            }
        }

        [Test]
        public void Encode_Test3()
        {
            var referenceBytes = new byte[] { 
                (byte)(Masks.LENGTH_LONG_FORM_MASK | (2 & Masks.LENGTH_MASK)),
                0x01,
                0xB3
            };

            var length = new BERLength(BERLengthForm.Long, 435);

            using (var ms = new MemoryStream())
            {
                length.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(referenceBytes, ms.ToArray()));
            }
        }

        [Test]
        public void Encode_Test4()
        {
            var length = new BERLength(BERLengthForm.Long, 1024);

            using (var ms = new ByteArrayStream())
            {
                length.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x82, 0x04, 0x00 }, ms.ToArray()));
            }
        }
    }
}