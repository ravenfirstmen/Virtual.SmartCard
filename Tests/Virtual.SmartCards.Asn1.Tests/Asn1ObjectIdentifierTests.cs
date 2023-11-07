using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Asn1.Types;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: OID")]
    public class Asn1ObjectIdentifierTests
    {
        #region Decoding

        [TestFixture]
        public class Asn1ObjectIdentifierEncodingTests : Asn1ObjectIdentifierTests
        {
            [Test]
            public void Encoding_Test1()
            {
                using (var ms = new ByteArrayStream())
                {
                    var oid = new Asn1ObjectIdentifier(new uint[] { 2, 100, 3 });
                    oid.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x81, 0x34, 0x03 }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Long, oid.Length.Form);
                    Assert.AreEqual(3, oid.Length.Value);
                }
            }

            [Test]
            public void Encoding_Test2()
            {
                using (var ms = new ByteArrayStream())
                {
                    var oid = new Asn1ObjectIdentifier(new uint[] { 2, 113469 });
                    oid.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x86, 0xF7, 0x0D }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Long, oid.Length.Form);
                    Assert.AreEqual(3, oid.Length.Value);
                }
            }

            [Test]
            public void Encoding_Test3()
            {
                using (var ms = new ByteArrayStream())
                {
                    var oid = new Asn1ObjectIdentifier(new uint[] { 2, 10000, 840, 135119, 9, 2, 12301002, 12132323, 191919, 2 });
                    oid.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0xCE, 0x60, 0x86, 0x48, 0x88, 0x9F, 0x4F, 0x09, 0x02, 0x85, 0xEE, 0xE5, 0x4A, 0x85, 0xE4, 0xBF, 0x63, 0x8B, 0xDB, 0x2F, 0x02 }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Long, oid.Length.Form);
                    Assert.AreEqual(21, oid.Length.Value);
                }
            }
        }

        #endregion

        #region Decoding

        [TestFixture]
        public class Asn1ObjectIdentifierDecodingTests : Asn1ObjectIdentifierTests
        {
            [Test]
            public void Decoding_Test1()
            {
                var b = new byte[] { 0x81, 0x34, 0x03 };

                using (var ms = new ByteArrayStream(b))
                {
                    var oid = new Asn1ObjectIdentifier(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    oid.Decode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(oid.Value, new uint[] { 2, 100, 3 }));
                    Assert.AreEqual("2.100.3", oid.ToString());
                }
            }

            [Test]
            public void Decoding_Test2()
            {
                var b = new byte[] { 0x86, 0xF7, 0x0D };

                using (var ms = new ByteArrayStream(b))
                {
                    var oid = new Asn1ObjectIdentifier(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    oid.Decode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(oid.Value, new uint[] { 2, 113469 }));
                    Assert.AreEqual("2.113469", oid.ToString());
                }
            }

            [Test]
            public void Decoding_Test3()
            {
                var b = new byte[] { 0x83, 0x80, 0x00 };

                using (var ms = new ByteArrayStream(b))
                {
                    var oid = new Asn1ObjectIdentifier(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    oid.Decode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(oid.Value, new uint[] { 2, 49072 }));
                    Assert.AreEqual("2.49072", oid.ToString());
                }
            }

            [Test]
            public void Decoding_Test4()
            {
                var b = new byte[] { 0x80, 0x85, 0x03 };

                using (var ms = new ByteArrayStream(b))
                {
                    var oid = new Asn1ObjectIdentifier(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    oid.Decode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(oid.Value, new uint[] { 2, 563 }));
                    Assert.AreEqual("2.563", oid.ToString());
                }
            }

            [Test]
            public void Decoding_Test5()
            {
                var b = new byte[] { 0xCE, 0x60, 0x86, 0x48, 0x88, 0x9F, 0x4F, 0x09, 0x02, 0x85, 0xEE, 0xE5, 0x4A, 0x85, 0xE4, 0xBF, 0x63, 0x8B, 0xDB, 0x2F, 0x02 };

                using (var ms = new ByteArrayStream(b))
                {
                    var oid = new Asn1ObjectIdentifier(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    oid.Decode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(oid.Value, new uint[] { 2, 10000, 840, 135119, 9, 2, 12301002, 12132323, 191919, 2 }));
                    Assert.AreEqual("2.10000.840.135119.9.2.12301002.12132323.191919.2", oid.ToString());
                }
            }
        }

        #endregion
    }
}