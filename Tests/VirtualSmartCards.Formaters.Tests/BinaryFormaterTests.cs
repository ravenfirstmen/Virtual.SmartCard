using NUnit.Framework;
using Virtual.SmartCard;
using Virtual.SmartCard.Formaters;

namespace VirtualSmartCards.Formaters.Tests
{
    [TestFixture]
    [Category("Binary formater")]
    public class BinaryFormaterTests
    {
        [Test]
        public void Test1()
        {
            var formater = new BinaryFormater((byte) 2);
            Assert.AreEqual("00000010", formater.ToString());
            Assert.AreEqual(sizeof(byte) * Constants.BITS_PER_BYTE, formater.ToString().Length);
        }

        [Test]
        public void Test2()
        {
            var formater = new BinaryFormater((short)1234);
            Assert.AreEqual("0000010011010010", formater.ToString());
            Assert.AreEqual(sizeof(short) * Constants.BITS_PER_BYTE, formater.ToString().Length);
        }

        [Test]
        public void Test3()
        {
            var formater = new BinaryFormater(1341467);
            Assert.AreEqual("00000000000101000111100000011011", formater.ToString());
            Assert.AreEqual(sizeof(int) * Constants.BITS_PER_BYTE, formater.ToString().Length);
        }

        [Test]
        public void Test4()
        {
            var formater = new BinaryFormater(13414679996798942L);
            Assert.AreEqual("0000000000101111101010001001010010010011111111000001011111011110", formater.ToString());
            Assert.AreEqual(sizeof(long) * Constants.BITS_PER_BYTE, formater.ToString().Length);
        }
    }
}