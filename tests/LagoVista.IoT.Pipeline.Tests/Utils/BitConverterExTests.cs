using LagoVista.IoT.Pipeline.Standard.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Tests.Utils
{
    [TestClass]
    public class BitConverterExTests
    {
        [TestMethod]
        public void Int16BigEndianTest()
        {
            var result = BitConverterEx.ToInt16FromBigEndian(new byte[] { 0x01, 0x00 });
            Assert.AreEqual(0x100, result);
        }

        [TestMethod]
        public void Int16BigEndianNegativeTest()
        {
            var result = BitConverterEx.ToInt16FromBigEndian(new byte[] { 0xFF, 0xF0 });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void Int16LittleEndianTest()
        {
            var result = BitConverterEx.ToInt16FromLittleEndian(new byte[] { 0x0, 0x01 });
            Assert.AreEqual(0x100, result);
        }

        [TestMethod]
        public void Int16LittleEndianTestNegative()
        {
            var result = BitConverterEx.ToInt16FromLittleEndian(new byte[] { 0xF0, 0xFF });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void UInt16BigEndian()
        {
            var result = BitConverterEx.ToUInt16FromBigEndian(new byte[] { 0xF0, 0xFF });
            Assert.AreEqual(61695, result);
        }

        [TestMethod]
        public void UInt16LittleEndian()
        {
            var result = BitConverterEx.ToUInt16FromLittleEndian(new byte[] { 0xF0, 0xFF });
            Assert.AreEqual(65520, result);
        }



        [TestMethod]
        public void Int32BigEndianTest()
        {
            var result = BitConverterEx.ToInt32FromBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00 });
            Assert.AreEqual(16777216, result);
        }

        [TestMethod]
        public void Int32BigEndianNegativeTest()
        {
            var result = BitConverterEx.ToInt32FromBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xF0 });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void Int32LittleEndianTest()
        {
            var result = BitConverterEx.ToInt32FromLittleEndian(new byte[] {0x00, 0x00, 0x0, 0x01 });
            Assert.AreEqual(16777216, result);
        }

        [TestMethod]
        public void Int32LittleEndianTestNegative()
        {
            var result = BitConverterEx.ToInt32FromLittleEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void UInt32BigEndian()
        {
            var result = BitConverterEx.ToUInt32FromBigEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(4043309055, result);
        }

        [TestMethod]
        public void UInt32LittleEndian()
        {
            var result = BitConverterEx.ToUInt32FromLittleEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(4294967280, result);
        }



        [TestMethod]
        public void Int64BigEndianTest()
        {
            var result = BitConverterEx.ToInt64FromBigEndian(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            Assert.AreEqual(72057594037927936, result);
            Assert.AreEqual(0x100000000000000, result);
        }

        [TestMethod]
        public void Int64BigEndianNegativeTest()
        {
            var result = BitConverterEx.ToInt64FromBigEndian(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0 });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void Int64LittleEndianTest()
        {
            var result = BitConverterEx.ToInt64FromLittleEndian(new byte[] { 0x00, 0x00, 0x00,0x00, 0x00, 0x00, 0x00, 0x01 });
            Assert.AreEqual(72057594037927936, result);
            Assert.AreEqual(0x100000000000000, result);
        }

        [TestMethod]
        public void Int64LittleEndianTestNegative()
        {
            var result = BitConverterEx.ToInt64FromLittleEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(-16, result);
        }

        [TestMethod]
        public void UInt64BigEndian()
        {
            var result = BitConverterEx.ToUInt64FromBigEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(17365880163140632575, result);
            Assert.AreEqual(0xF0FFFFFFFFFFFFFF, result);
        }

        [TestMethod]
        public void UInt64LittleEndian()
        {
            var result = BitConverterEx.ToUInt64FromLittleEndian(new byte[] { 0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            Assert.AreEqual(18446744073709551600, result);
            Assert.AreEqual(0xFFFFFFFFFFFFFFF0, result);
        }

    }
}
