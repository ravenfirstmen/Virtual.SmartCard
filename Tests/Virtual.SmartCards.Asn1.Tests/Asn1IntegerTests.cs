using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Asn1.Types;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: Integer")]
    public class Asn1IntegerTests
    {
        #region Encoding

        [TestFixture]
        public class Asn1IntegerEncodingTests : Asn1IntegerTests
        {
            [Test]
            public void Encoding_Test1()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(51);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x33 }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Short, integer.Length.Form);
                    Assert.AreEqual(1, integer.Length.Value);
                }
            }

            [Test]
            public void Encoding_Test2()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(256);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x01, 0x00 }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Long, integer.Length.Form);
                    Assert.AreEqual(2, integer.Length.Value);
                }
            }

            [Test]
            public void Encoding_Test3()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(0);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x00 }, ms.ToArray()));
                }
            }

            [Test]
            public void Encoding_Test4()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(127);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x7f }, ms.ToArray()));
                }
            }

            [Test]
            public void Encoding_Test5()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(128);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x00, 0x80 }, ms.ToArray()));
                }

            }

            [Test]
            public void Encoding_Test6()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(-128);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x80 }, ms.ToArray()));
                }
            }

            [Test]
            public void Encoding_Test7()
            {
                using (var ms = new ByteArrayStream())
                {
                    var integer = new Asn1Integer(-129);
                    integer.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0xff, 0x7f }, ms.ToArray()));
                }
            }
        }

        #endregion

        #region Decoding

        [TestFixture]
        public class Asn1IntegerDecodingTests : Asn1IntegerTests
        {

            [Test]
            public void Decoding_Test1()
            {
                var b = new byte[] { 0x33 };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Short, 1));
                    integer.Decode(ms);

                    Assert.AreEqual(51, integer.Value);
                }
            }


            [Test]
            public void Decoding_Test2()
            {
                var b = new byte[] { 0x03, 0x07 };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, 2));
                    integer.Decode(ms);

                    Assert.AreEqual(775, integer.Value);
                }
            }

            [Test]
            public void Decoding_Test3()
            {
                var b = new byte[] { 0x80 };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, 1));
                    integer.Decode(ms);

                    Assert.AreEqual(-128, integer.Value);
                }
            }

            [Test]
            public void Decoding_Test4()
            {
                var b = new byte[] { 0xff, 0x7f };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, 2));
                    integer.Decode(ms);

                    Assert.AreEqual(-129, integer.Value);
                }
            }

            //
            [Test]
            public void Decoding_Test5()
            {
                var b = new byte[] { 0xff };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, 1));
                    integer.Decode(ms);

                    Assert.AreEqual(-1, integer.Value);
                }
            }

            [Test]
            public void Decoding_Test6()
            {
                var b = new byte[] { 0xFF, 0x78 };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, 2));
                    integer.Decode(ms);

                    Assert.AreEqual(-136, integer.Value);
                }
            }

            [Test]
            public void Decoding_Test7()
            {
                var b = new byte[] { 0x80, 0x00, 0x01 };

                using (var ms = new ByteArrayStream(b))
                {
                    var integer = new Asn1Integer(new Asn1Length(Asn1LengthForm.Long, (ulong)b.Length));
                    integer.Decode(ms);

                    Assert.AreEqual(-8388607, integer.Value);
                }
            }

        }
        #endregion

    }
}