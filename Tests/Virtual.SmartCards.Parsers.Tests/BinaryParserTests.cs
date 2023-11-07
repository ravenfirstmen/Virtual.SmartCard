using NUnit.Framework;
using Virtual.SmartCard.Parsers;

namespace Virtual.SmartCards.Parsers.Tests
{
    public class BinaryParserTests
    {
        [TestFixture]
        [Category("Binaryparser parser")]
        public class BinaryParsersTests
        {
            [Test]
            public void Test1()
            {
                var parser = new BinaryParser();
                Assert.AreEqual(0x02, parser.ParseToByte("00000010"));
            }

            [Test]
            public void Test2()
            {
                var parser = new BinaryParser();
                Assert.AreEqual(1234, parser.ParseToInt16("0000010011010010"));
            }

            [Test]
            public void Test3()
            {
                var parser = new BinaryParser();
                Assert.AreEqual(1341467, parser.ParseToInt32("00000000000101000111100000011011"));
            }

            [Test]
            public void Test4()
            {
                var parser = new BinaryParser();
                Assert.AreEqual(13414679996798942L, parser.ParseToInt64("0000000000101111101010001001010010010011111111000001011111011110"));
            }
        }         
    }
}