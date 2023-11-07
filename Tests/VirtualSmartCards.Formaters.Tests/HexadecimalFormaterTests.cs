using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Virtual.SmartCard.Formaters;

namespace VirtualSmartCards.Formaters.Tests
{
    [TestFixture]
    [Category("Hexadecimal formater")]
    public class HexadecimalFormaterTests
    {
        [Test]
        public void Test1()
        {
            var formater = new HexadecimalFormater(true);
            Assert.AreEqual("01", formater.ToString());
            Assert.AreEqual(sizeof(byte) * 2, formater.ToString().Length);
        }

        [Test]
        public void Test2()
        {
            var formater = new HexadecimalFormater((short)1234);
            Assert.AreEqual("04D2", formater.ToString());
            Assert.AreEqual(sizeof(short) * 2, formater.ToString().Length);
        }

        [Test]
        public void Test3()
        {
            var formater = new HexadecimalFormater(1341467);
            Assert.AreEqual("0014781B", formater.ToString());
            Assert.AreEqual(sizeof(int) * 2, formater.ToString().Length);
        }

        [Test]
        public void Test4()
        {
            var formater = new HexadecimalFormater(13414679996798942L);
            Assert.AreEqual("002FA89493FC17DE", formater.ToString());
            Assert.AreEqual(sizeof(long) * 2, formater.ToString().Length);
        }
    }
}

