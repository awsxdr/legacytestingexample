namespace LegacyTestingExample.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Moq;

    using NUnit.Framework;

    using UnitTestingTools;

    [TestFixture]
    public class FlightFinderTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // MessageBox.Show
            MethodReplacer.ReplaceMethod(
                () => MessageBox.Show(Parameter.Of<string>()),
                () => MockMethods.MessageBoxShow(Parameter.Of<string>()));

            // GlobalStuff.Conn
            MethodReplacer.ReplaceMethod(
                typeof(GlobalStuff).GetMethod("get_Conn"),
                typeof(MockMethods).GetMethod("get_DatabaseConnection"));

            // ResultsForm.Show
            MethodReplacer<ResultsPresenter, MockMethods>.ReplaceMethod(
                dst => dst.ShowResults(Parameter.Of<Flight>(), Parameter.Of<Flight>()),
                src => src.ResultsPresenterShowResults(Parameter.Of<Flight>(), Parameter.Of<Flight>()));
        }

        [SetUp]
        public void SetUp()
        {
            MockMethods.Reset();
        }

        [Test]
        public void FindFlightsReturnsFalseOnNullDeparture()
        {
            var classUnderTest = new FlightFinder();

            var result = classUnderTest.FindFlights(null, new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightsReturnsFalseOnNullDestination()
        {
            var classUnderTest = new FlightFinder();

            var result = classUnderTest.FindFlights(new Airport(), null, DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightsReturnsFalseOnInvalidOutAndBackDates()
        {
            var classUnderTest = new FlightFinder();

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(14), DateTime.Now.AddDays(7));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightsReturnsFalseOnOutDateInPast()
        {
            var classUnderTest = new FlightFinder();

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightsReturnsTrueOnSuccess()
        {
            var classUnderTest = new FlightFinder();

            var flightTimeManagerMock = new Mock<IFlightTimeManager>();

            flightTimeManagerMock
                .Setup(m => m.GetFlightsForDate(It.IsAny<DateTime>()))
                .Returns(new[] {new Flight {Airline = "TST"}});

            ReflectionHelper.SetPrivateField("_flightTimeManager", classUnderTest, flightTimeManagerMock.Object);

            var databaseConnectionMock = new Mock<IDatabaseConnection>();

            MockMethods.DatabaseConnection = databaseConnectionMock.Object;

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            Assert.IsTrue(result);
        }

        [Test]
        public void FindFlightsAttemptsToReserveBothFlights()
        {
            var classUnderTest = new FlightFinder();

            var flightTimeManagerMock = new Mock<IFlightTimeManager>();

            flightTimeManagerMock
                .Setup(m => m.GetFlightsForDate(It.IsAny<DateTime>()))
                .Returns(new[] { new Flight { Airline = "TST" } });

            ReflectionHelper.SetPrivateField("_flightTimeManager", classUnderTest, flightTimeManagerMock.Object);

            var databaseConnectionMock = new Mock<IDatabaseConnection>();

            MockMethods.DatabaseConnection = databaseConnectionMock.Object;

            classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            databaseConnectionMock.Verify(
                m => m.ReserveFlight(It.IsAny<Flight>()),
                Times.Exactly(2));
        }

        [Test]
        public void FindFlightsReturnsFalseWhenNoFlightsFound()
        {
            var classUnderTest = new FlightFinder();

            var flightTimeManagerMock = new Mock<IFlightTimeManager>();

            flightTimeManagerMock
                .Setup(m => m.GetFlightsForDate(It.IsAny<DateTime>()))
                .Returns(() => new[] { new Flight { Airline = Guid.NewGuid().ToString() } });

            ReflectionHelper.SetPrivateField("_flightTimeManager", classUnderTest, flightTimeManagerMock.Object);

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightsReturnsFalseWhenErrorReservingFlights()
        {
            var classUnderTest = new FlightFinder();

            var flightTimeManagerMock = new Mock<IFlightTimeManager>();

            flightTimeManagerMock
                .Setup(m => m.GetFlightsForDate(It.IsAny<DateTime>()))
                .Returns(new[] { new Flight { Airline = "TST" } });

            ReflectionHelper.SetPrivateField("_flightTimeManager", classUnderTest, flightTimeManagerMock.Object);

            var databaseConnectionMock = new Mock<IDatabaseConnection>();

            databaseConnectionMock
                .Setup(m => m.ReserveFlight(It.IsAny<Flight>()))
                .Throws<Exception>();

            MockMethods.DatabaseConnection = databaseConnectionMock.Object;

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        [Test]
        public void FindFlightChecksSurroundingDatesWhenCheckAlternativesIsTrue()
        {
            var classUnderTest = new FlightFinder();

            var flightTimeManagerMock = new Mock<IFlightTimeManager>();

            flightTimeManagerMock
                .Setup(m => m.GetFlightsForDate(It.IsAny<DateTime>()))
                .Returns(() => new[] { new Flight { Airline = Guid.NewGuid().ToString() } });

            ReflectionHelper.SetPrivateField("_flightTimeManager", classUnderTest, flightTimeManagerMock.Object);

            ReflectionHelper.InvokePrivateMethod("set_CheckAlternatives", classUnderTest, true);

            var result = classUnderTest.FindFlights(new Airport(), new Airport(), DateTime.Now.AddDays(7), DateTime.Now.AddDays(14));

            flightTimeManagerMock.Verify(
                    m => m.GetFlightsForDate(It.IsAny<DateTime>()),
                    Times.Exactly(2 * 3 * 3));

            Assert.AreEqual(1, MockMethods.GetCallCount("MessageBoxShow"));
            Assert.IsFalse(result);
        }

        private class MockMethods
        {
            private static readonly Dictionary<string, int> CalledMethods = new Dictionary<string, int>();

            public static IDatabaseConnection DatabaseConnection { get; set; }

            public static DialogResult MessageBoxShow(string message)
            {
                IncrementMethodCallCount(nameof(MessageBoxShow));

                return DialogResult.OK;
            }

            public void ResultsPresenterShowResults(Flight outFlight, Flight backFlight)
            {
                IncrementMethodCallCount(nameof(ResultsPresenterShowResults));
            }

            private static void IncrementMethodCallCount(string methodName) =>
                CalledMethods[methodName] = GetCallCount(methodName) + 1;

            public static int GetCallCount(string methodName) =>
                CalledMethods.ContainsKey(methodName)
                    ? CalledMethods[methodName]
                    : 0;

            public static void Reset()
            {
                CalledMethods.Clear();
            }
        }
    }
}
