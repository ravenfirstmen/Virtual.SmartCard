using System.IO;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: TAG")]
    public class Asn1TAGTests
    {
        #region Decoding

        [TestFixture]
        public class Asn1TAGDecodingTests : Asn1TAGTests
        {
            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_Empty_Stream()
            {
                //var tag = new Asn1Tag(Asn1Class.Application, Asn1EncodingForm.Primitive, Asn1Type.Integer, 0x02);

                using (var ms = new MemoryStream())
                {
                    Asn1Tag.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_UniversalClass_And_LongForm()
            {
                var b = new byte[] { (byte)Asn1Class.Universal << 6 | (byte)Asn1EncodingForm.Primitive << 5 | (byte)Asn1Type.LongForm };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Tag.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_NextEOF()
            {
                var b = new byte[] { (byte)Asn1Class.Application << 6 | (byte)Asn1EncodingForm.Primitive << 5 | (byte)Asn1Type.LongForm };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Tag.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_MSB_First_Subsquent_Byte_Is_0()
            {
                var b = new byte[]
                {
                    (byte)Asn1Class.Application << 6 | (byte)Asn1EncodingForm.Primitive << 5 | (byte)Asn1Type.LongForm,
                    0x00
                };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Tag.Decode(ms);
                }

            }

            [Test]
            [ExpectedException(typeof(Asn1FormatException))]
            public void Decoding_Should_Throw_Asn1FormatException_On_LongForm_And_More_Then_9_Bytes_for_Encoding()
            {
                var b = new byte[]
                {
                    (byte)Asn1Class.Application << 6 | (byte)Asn1EncodingForm.Primitive << 5 | (byte)Asn1Type.LongForm,
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)(Masks.TAG_NUMAsn1_MASK | Masks.HAS_TAG_NUMAsn1),
                    (byte)Masks.TAG_NUMAsn1_MASK
                };
                using (var ms = new MemoryStream(b))
                {
                    Asn1Tag.Decode(ms);
                }
            }

            [Test]
            public void Decoding_Test1()
            {
                var b = new byte[]
                {
                     91
                };
                using (var ms = new MemoryStream(b))
                {
                    var tag = Asn1Tag.Decode(ms);

                    Assert.AreEqual(Asn1Class.Application, tag.Class);
                    Assert.AreEqual(Asn1EncodingForm.Primitive, tag.EncodingForm);
                    Assert.AreEqual(Asn1Type.GeneralString, tag.Type);
                    Assert.AreEqual(0x1B, tag.TagNumAsn1);
                }
            }

        }

        #endregion

        #region Encoding

        [TestFixture]
        public class Asn1TAGEncodingTests : Asn1TAGTests
        {

            [Test]
            public void Encode_Test1()
            {
                var tag = new Asn1Tag(Asn1Class.Application, Asn1EncodingForm.Primitive, Asn1Type.LongForm, 15);

                using (var ms = new MemoryStream())
                {
                    tag.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x4F }, ms.ToArray()));
                }
            }

            [Test]
            public void Encode_Test2()
            {
                var tag = new Asn1Tag(Asn1Class.Application, Asn1EncodingForm.Primitive, Asn1Type.LongForm, 31);

                using (var ms = new MemoryStream())
                {
                    tag.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x5F, 0x1F }, ms.ToArray()));
                }
            }

            [Test]
            public void Encode_Test3()
            {
                var tag = new Asn1Tag(Asn1Class.Application, Asn1EncodingForm.Primitive, Asn1Type.LongForm, 201);

                using (var ms = new MemoryStream())
                {
                    tag.Encode(ms);

                    Assert.AreEqual(true, ByteArrayUtils.AreEqual(new byte[] { 0x5F, 0x81, 0x49 }, ms.ToArray()));
                }
            }

        }

        #endregion
    }
}