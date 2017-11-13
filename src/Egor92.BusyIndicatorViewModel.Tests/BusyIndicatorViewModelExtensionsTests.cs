using System;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace Egor92.Tests
{
    [TestFixture]
    public class BusyIndicatorViewModelExtensionsTests
    {
        #region Fields

        private TestScheduler _testScheduler;
        private BusyIndicatorViewModel _busyIndicatorVM;

        #endregion

        #region Administrative

        [SetUp]
        public void SetUp()
        {
            _testScheduler = new TestScheduler();
        }

        [TearDown]
        public void Dispose()
        {
            _busyIndicatorVM?.Dispose();
        }

        #endregion

        [Test]
        public void ExecuteBeingBusy_WhenCallMethod_ThenInvokeAction()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act
            bool actionInvoked = false;
            _busyIndicatorVM.ExecuteBeingBusy(_testScheduler, _ =>
            {
                actionInvoked = true;
            });

            _testScheduler.Start();

            //Assert
            Assert.That(actionInvoked, Is.True);
        }

        [Test]
        public void ExecuteBeingBusy_WhenMethodIsExecutingAndDelayTimePassed_ThenIsBusyEqualsTrueInsideAction()
        {
            //Arrange
            TimeSpan delayTime = TimeSpan.FromMilliseconds(100);
            _busyIndicatorVM = new BusyIndicatorViewModel(delayTime, _testScheduler);

            //Act
            _busyIndicatorVM.ExecuteBeingBusy(_testScheduler, _ =>
            {
                _testScheduler.AdvanceBy(delayTime);
                
                //Assert
                Assert.That(_busyIndicatorVM.IsBusy, Is.True);
            });
        }
    }
}
