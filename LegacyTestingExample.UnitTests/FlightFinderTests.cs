using System;
using System.Windows.Forms;
using NUnit.Framework;
using UnitTestingTools;

namespace LegacyTestingExample.UnitTests
{
    [TestFixture]
    public class FlightFinderTests
    {
        private string message;
        private DialogResult LogMessage(string msg)
        {
            message = msg;
            return DialogResult.OK;
        }

        [Test]
        void FlightFinderTest()
        {
            var ff = new FlightFinder();

            MethodReplacer.ReplaceMethod(() => MessageBox.Show(Parameter.Of<string>()), () => LogMessage(Parameter.Of<string>()));

            var src = new Airport();
            src.Code = "1";
            var dest = new Airport();
            dest.Code = "2";

            DateTime outDt = DateTime.MinValue;
            DateTime backDt = DateTime.MaxValue;
            var res = ff.FindFlights(src, dest, outDt, backDt);

            Assert.True(res);
            Assert.AreEqual(message, "No departure or destination airport");
        }
    }
}
