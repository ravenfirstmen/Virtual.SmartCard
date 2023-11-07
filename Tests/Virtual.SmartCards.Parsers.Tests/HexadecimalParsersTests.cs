using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Virtual.SmartCard.Parsers;

namespace Virtual.SmartCards.Parsers.Tests
{
    [TestFixture]
    [Category("Hexadecimal parser")]
    public class HexadecimalParsersTests
    {
        [Test]
        public void Test1()
        {
            var parser = new HexadecimalParser();
            Assert.AreEqual(new byte[] { 0x0A }, parser.Parse("A"));
        }

        [Test]
        public void Test2()
        {
            var parser = new HexadecimalParser();
            Assert.AreEqual(new byte[] { 0x01, 0x01, 0x42, 0x70, 0x40, 0x57, 0x13 }, parser.Parse("01014270405713"));
        }
    
        [Test]
        public void Test3()
        {
            var parser = new HexadecimalParser();
            Assert.AreEqual(16, parser.ParseToInt16("10"));
        }
    }
}
