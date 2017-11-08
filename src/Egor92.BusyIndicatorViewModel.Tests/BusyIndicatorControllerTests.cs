using System;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace Egor92.BusyIndicatorViewModel.Tests
{
    [TestFixture]
    public class BusyIndicatorControllerTests
    {
        private TestScheduler _testScheduler;
        private BusyIndicatorController _busyIndicatorVM;

        [SetUp]
        public void SetUp()
        {
            _testScheduler = new TestScheduler();
        }

        [Test]
        public void SetBusy_WhenSetBusyAndDelayTimePassed_ThenIsBusyEqualsTrue()
        {
            //Arrange
            TimeSpan delayTime = TimeSpan.FromMilliseconds(100);
            _busyIndicatorVM = new BusyIndicatorController(delayTime, _testScheduler);

            //Act
            _busyIndicatorVM.SetBusy();
            _testScheduler.AdvanceBy(delayTime.Ticks);

            //Assert
            Assert.That(_busyIndicatorVM.IsBusy, Is.True);
        }
    }
}
