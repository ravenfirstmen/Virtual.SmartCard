using System;
using NUnit.Framework;
using Virtual.SmartCard.TLV.Asn1;
using Virtual.SmartCard.TLV.Asn1.Types;
using Virtual.SmartCards.Asn1.Tests.Utils;

namespace Virtual.SmartCards.Asn1.Tests
{
    [TestFixture]
    [Category("Asn1 :: Asn1 :: Real")]
    public class Asn1RealTests
    {
        #region Decoding

        [TestFixture]
        public class Asn1RealDecodingTests : Asn1RealTests
        {
            [Test]
            public void Decoding_Test1()
            {
                var b = new byte[] { 0x80, 0xFB, 0x05 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test2()
            {
                var b = new byte[] { 0x90, 0xFE, 0x0A };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test3()
            {
                var b = new byte[] { 0xAC, 0xFE, 0x05 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test4()
            {
                var b = new byte[] { 0x94, 0xFE, 0x05 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test5()
            {
                var b = new byte[] { 0x83, 0x04, 0xff, 0xff, 0xff, 0xfb, 0x05 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test6()
            {
                var b = new byte[] { 0x83, 0x04, 0x7f, 0xff, 0xff, 0xfb, 0x05 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(0.15625, real.Value);
                }
            }

            [Test]
            public void Decoding_Test7()
            {
                var b = new byte[] { 0x40 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(Double.PositiveInfinity, real.Value);
                }
            }

            [Test]
            public void Decoding_Test8()
            {
                var b = new byte[] { 0x41 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(Double.NegativeInfinity, real.Value);
                }
            }

            [Test]
            public void Decoding_Test9()
            {
                var b = new byte[] { 0x42 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(Double.NaN, real.Value);
                }
            }

            [Test]
            public void Decoding_Test10()
            {
                var b = new byte[] { 128, 11, 4, 77 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(2254848.0, real.Value);
                }
            }

            [Test]
            public void Decoding_Test11()
            {
                var b = new byte[] { 0x80, 0xFF, 0x33 };

                using (var ms = new ByteArrayStream(b))
                {
                    var real = new Asn1Real(new Asn1Length(Asn1LengthForm.Short, (ulong)b.Length));
                    real.Decode(ms);

                    Assert.AreEqual(51.0, real.Value);
                }
            }

        }

        #endregion

    }
}