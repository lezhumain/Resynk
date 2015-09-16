using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resynk;

namespace ResynkTest
{
    [TestClass]
    public class TimeTest
    {
        private Time _time;

        private void ResetTime()
        {
            _time = new Time(0, 0, 0, 0);
        }

        private void FillTime()
        {
            _time = new Time(59, 59, 59, 999);
        }

        [TestInitialize]
        public void TestInit()
        {
            this.ResetTime();
        }

        [TestCleanup]
        public void TestClean()
        {
        }

        [TestMethod]
        public void TestAddMil()
        {
            ResetTime();

            _time.AddMil(600);
            Assert.AreEqual("00:00:00,600", _time.ToString());
            
            _time.AddMil(1000);
            Assert.AreEqual("00:00:01,600", _time.ToString());
            
            _time.AddMil(60000);
            Assert.AreEqual("00:01:01,600", _time.ToString());

            _time.AddMil(3600000);
            Assert.AreEqual("01:01:01,600", _time.ToString());
        }

        [TestMethod]
        public void TestAddSec()
        {
            ResetTime();

            _time.AddSec(6);
            Assert.AreEqual("00:00:06,000", _time.ToString());

            _time.AddSec(60);
            Assert.AreEqual("00:01:06,000", _time.ToString());

            _time.AddSec(3600);
            Assert.AreEqual("01:01:06,000", _time.ToString());
        }

        [TestMethod]
        public void TestAddMin()
        {
            ResetTime();

            _time.AddMin(6);
            Assert.AreEqual("00:06:00,000", _time.ToString());

            _time.AddMin(60);
            Assert.AreEqual("01:06:00,000", _time.ToString());
        }

        [TestMethod]
        public void TestAddHeu()
        {
            ResetTime();

            _time.AddHeu(6);
            Assert.AreEqual("06:00:00,000", _time.ToString());

            _time.AddHeu(60);
            Assert.AreEqual("66:00:00,000", _time.ToString());
        }

        [TestMethod]
        public void TestSubMil()
        {
            FillTime();

            _time.AddMil(-5);
            Assert.AreEqual("59:59:59,994", _time.ToString());

            _time.AddMil(-1000);
            Assert.AreEqual("59:59:58,994", _time.ToString());

            _time.AddMil(-60000);
            Assert.AreEqual("59:58:58,994", _time.ToString());

            _time.AddMil(-3600000);
            Assert.AreEqual("58:58:58,994", _time.ToString());
        }

        [TestMethod]
        public void TestSubSec()
        {
            FillTime();

            _time.AddSec(-6);
            Assert.AreEqual("59:59:53,999", _time.ToString());

            _time.AddSec(-60);
            Assert.AreEqual("59:58:53,999", _time.ToString());

            _time.AddSec(-3600);
            Assert.AreEqual("58:58:53,999", _time.ToString());

            _time.AddSec(-216000);
            Assert.AreEqual("00:00:00,000", _time.ToString());
        }

        [TestMethod]
        public void TestSubMin()
        {
            FillTime();

            _time.AddMin(-6);
            Assert.AreEqual("59:53:59,999", _time.ToString());

            _time.AddMin(-60);
            Assert.AreEqual("58:53:59,999", _time.ToString());

            _time.AddMin(-3600);
            Assert.AreEqual("00:00:00,000", _time.ToString());
        }

        [TestMethod]
        public void TestSubHeu()
        {
            FillTime();

            _time.AddHeu(-6);
            Assert.AreEqual("53:59:59,999", _time.ToString());

            _time.AddHeu(-53);
            Assert.AreEqual("00:59:59,999", _time.ToString());

            _time.AddHeu(-60);
            Assert.AreEqual("00:00:00,000", _time.ToString());
        }
    }
}
