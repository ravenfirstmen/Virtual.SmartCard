using System.IO;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Asn1.Types;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: Boolean")]
    public class Asn1BooleanTests
    {
        #region Encoding

        [TestFixture]
        public class Asn1BooleanEncodingTests : Asn1BooleanTests
        {
            [Test]
            public void Encoding_Test1()
            {
                using (var ms = new ByteArrayStream())
                {
                    var boolean = new Asn1Boolean(true);
                    boolean.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0xFF }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Short, boolean.Length.Form);
                    Assert.AreEqual(1, boolean.Length.Value);
                }
            }

            [Test]
            public void Encoding_Test2()
            {
                using (var ms = new ByteArrayStream())
                {
                    var boolean = new Asn1Boolean(false);
                    boolean.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x00 }, ms.ToArray()));
                    Assert.AreEqual(Asn1LengthForm.Short, boolean.Length.Form);
                    Assert.AreEqual(1, boolean.Length.Value);
                }
            }
        }

        #endregion

        #region Decoding

        [TestFixture]
        public class Asn1BooleanDecodingTests : Asn1BooleanTests
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
            public void Decoding_Test_1()
            {
                var b = new byte[] { 0xFF };
                using (var ms = new MemoryStream(b))
                {
                    var boolean = new Asn1Boolean();
                    boolean.Decode(ms);

                    Assert.AreEqual(true, boolean.Value);
                }

            }

            [Test]
            public void Decoding_Test_2()
            {
                var b = new byte[] { 0x00 };
                using (var ms = new MemoryStream(b))
                {
                    var boolean = new Asn1Boolean();
                    boolean.Decode(ms);

                    Assert.AreEqual(false, boolean.Value);
                }

            }

            [Test]
            public void Decoding_Test_3()
            {
                var b = new byte[] { 0x12 }; // qq um valor != 0 => true
                using (var ms = new MemoryStream(b))
                {
                    var boolean = new Asn1Boolean();
                    boolean.Decode(ms);

                    Assert.AreEqual(true, boolean.Value);
                }

            }

        }

        #endregion

    }
}