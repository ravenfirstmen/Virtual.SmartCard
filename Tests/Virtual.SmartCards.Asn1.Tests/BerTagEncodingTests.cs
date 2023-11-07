using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Simple;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: BER :: TAG :: Encoding")]
    public class BerTagEncodingTests
    {

        [Test]
        public void Encode_Test1()
        {
            var tag = new BERTag(BERClass.Application, BEREncodingForm.Primitive, BERType.LongForm, 15);

            using (var ms = new MemoryStream())
            {
                tag.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] {0x4F}, ms.ToArray()));
            }
        }

        [Test]
        public void Encode_Test2()
        {
            var tag = new BERTag(BERClass.Application, BEREncodingForm.Primitive, BERType.LongForm, 31);

            using (var ms = new MemoryStream())
            {
                tag.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x5F, 0x1F }, ms.ToArray()));
            }
        }

        [Test]
        public void Encode_Test3()
        {
            var tag = new BERTag(BERClass.Application, BEREncodingForm.Primitive, BERType.LongForm, 201);

            using (var ms = new MemoryStream())
            {
                tag.Encode(ms);

                Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x5F, 0x81, 0x49 }, ms.ToArray()));
            }
        }
    }
}
