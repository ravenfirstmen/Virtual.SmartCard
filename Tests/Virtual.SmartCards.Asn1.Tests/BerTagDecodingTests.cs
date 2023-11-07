using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: BER :: TAG :: Decoding")]
    public class BerTagDecodingTests
    {
        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_Empty_Stream()
        {
            //var tag = new BERTag(BERClass.Application, BEREncodingForm.Primitive, BERType.Integer, 0x02);

            using (var ms = new MemoryStream())
            {
                BERTag.Decode(ms);
            }
            
        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_UniversalClass_And_LongForm()
        {
            var b = new byte[] { (byte)BERClass.Universal << 6 | (byte)BEREncodingForm.Primitive << 5 | (byte)BERType.LongForm };
            using(var ms = new MemoryStream(b))
            {
                BERTag.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_NextEOF()
        {
            var b = new byte[] { (byte)BERClass.Application << 6 | (byte)BEREncodingForm.Primitive << 5 | (byte)BERType.LongForm };
            using (var ms = new MemoryStream(b))
            {
                BERTag.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_MSB_First_Subsquent_Byte_Is_0()
        {
            var b = new byte[]
                {
                    (byte)BERClass.Application << 6 | (byte)BEREncodingForm.Primitive << 5 | (byte)BERType.LongForm,
                    0x00
                };
            using (var ms = new MemoryStream(b))
            {
                BERTag.Decode(ms);
            }

        }

        [Test]
        [ExpectedException(typeof(BERFormatException))]
        public void Decoding_Should_Throw_BERFormatException_On_LongForm_And_More_Then_9_Bytes_for_Encoding()
        {
            var b = new byte[]
                {
                    (byte)BERClass.Application << 6 | (byte)BEREncodingForm.Primitive << 5 | (byte)BERType.LongForm,
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)(Masks.TAG_NUMBER_MASK | Masks.HAS_TAG_NUMBER),
                    (byte)Masks.TAG_NUMBER_MASK
                };
            using (var ms = new MemoryStream(b))
            {
                BERTag.Decode(ms);
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
                var tag = BERTag.Decode(ms);

                Assert.AreEqual(BERClass.Application, tag.Class);
                Assert.AreEqual(BEREncodingForm.Primitive, tag.EncodingForm);
                Assert.AreEqual(BERType.GeneralString, tag.Type);
                Assert.AreEqual(0x1B, tag.TagNumber);
            }
        }
    }
}
