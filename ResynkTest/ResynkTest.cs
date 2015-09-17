using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resynk;

namespace ResynkTest
{
    [TestClass]
    public class ResynkTest
    {
        [TestMethod]
        public void TestResynkLines()
        {
            const int milToAdd = 60000; // 1mn
            const string srt = "1+00:00:02,000 --> 00:00:05,520+Au revoir ! Enchanté de vous avoir+rencontré, désolé pour le dérangement.++2+00:00:05,520 --> 00:00:11,040+Vous pensiez que j'allais vous laisser+repartir sans moi,++3+00:00:11,040 --> 00:00:13,360+après ce que nous venons de traverser ?";
            const string synkedSrt = "1+00:01:02,000 --> 00:01:05,520+Au revoir ! Enchanté de vous avoir+rencontré, désolé pour le dérangement.++2+00:01:05,520 --> 00:01:11,040+Vous pensiez que j'allais vous laisser+repartir sans moi,++3+00:01:11,040 --> 00:01:13,360+après ce que nous venons de traverser ?";
            var expectedSrtA = synkedSrt.Split('+');

            var resy = new SrtResynk(milToAdd);
            var result = resy.ResynkLines(srt.Split('+'));
            var cpt = 0;

            Assert.IsTrue(result);
            Assert.AreEqual(expectedSrtA.Length, resy.TestContent.Count);

            for (; cpt < expectedSrtA.Length; cpt++)
            {
                Assert.AreEqual(expectedSrtA[cpt], resy.TestContent[cpt]);
            }
        }
    }
}
