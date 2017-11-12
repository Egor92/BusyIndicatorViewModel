using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;

namespace Egor92.Tests
{
    [TestFixture]
    public sealed class BusyIndicatorViewModelTests : IDisposable
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
        public void ImplementINotifyPropertyChanged()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act

            //Assert
            Assert.That(_busyIndicatorVM, Is.InstanceOf<INotifyPropertyChanged>());
        }

        #region SetBusyState

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetBusyState_WhenSetBusyStateAndDelayTimePassed_ThenIsBusyEqualsTrue(bool initialIsBusy)
        {
            //Arrange
            TimeSpan delayTime = TimeSpan.FromMilliseconds(100);
            _busyIndicatorVM = new BusyIndicatorViewModel(delayTime, _testScheduler);

            _busyIndicatorVM.SetBusyState(initialIsBusy);
            _testScheduler.AdvanceBy(delayTime);

            //Act
            bool newBusyState = !initialIsBusy;
            _busyIndicatorVM.SetBusyState(newBusyState);
            _testScheduler.AdvanceBy(delayTime);

            //Assert
            Assert.That(_busyIndicatorVM.IsBusy, Is.EqualTo(newBusyState));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetBusyState_WhenSetTheSameBusyStateAndDelayTimePassed_ThenDoesNotSetBusyState(bool initialIsBusy)
        {
            //Arrange
            TimeSpan delayTime = TimeSpan.FromMilliseconds(100);
            _busyIndicatorVM = new BusyIndicatorViewModel(delayTime, _testScheduler);

            _busyIndicatorVM.SetBusyState(initialIsBusy);
            _testScheduler.AdvanceBy(delayTime);

            //Act
            bool newBusyState = initialIsBusy;
            _busyIndicatorVM.SetBusyState(newBusyState);
            _testScheduler.AdvanceBy(delayTime);

            //Assert
            Assert.That(_busyIndicatorVM.IsBusy, Is.EqualTo(initialIsBusy));
        }

        #endregion

        #region IsBusy

        [Test]
        public void IsBusy_DefaultValueEqualsFalse()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act

            //Assert
            Assert.That(_busyIndicatorVM.IsBusy, Is.False);
        }

        #endregion

        #region Content

        [Test]
        public void Content_DefaultValueEqualsNull()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act

            //Assert
            Assert.That(_busyIndicatorVM.Content, Is.Null);
        }

        [Test]
        [AutoData]
        public void Content_WhenChangeContent_ThenRaisePropertyChanged(string newContent)
        {
            const string propertyName = nameof(BusyIndicatorViewModel.Content);

            void ChangeProperty(BusyIndicatorViewModel x)
            {
                x.Content = newContent;
            }

            WhenChangeProperty_ThenRaisePropertyChanged(propertyName, ChangeProperty);
        }

        #endregion

        #region Progress

        [Test]
        public void Progress_DefaultValueEqualsZero()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act

            //Assert
            Assert.That(_busyIndicatorVM.Progress, Is.EqualTo(0.0));
        }

        [Test]
        [AutoData]
        public void Progress_WhenChangeProgress_ThenRaisePropertyChanged(double newProgress)
        {
            const string propertyName = nameof(BusyIndicatorViewModel.Progress);

            void ChangeProperty(BusyIndicatorViewModel x)
            {
                x.Progress = newProgress;
            }

            WhenChangeProperty_ThenRaisePropertyChanged(propertyName, ChangeProperty);
        }

        #endregion

        #region IsIntermediate

        [Test]
        public void IsIntermediate_DefaultValueEqualsZero()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act

            //Assert
            Assert.That(_busyIndicatorVM.IsIntermediate, Is.False);
        }

        [Test]
        public void IsIntermediate_WhenChangeIsIntermediate_ThenRaisePropertyChanged()
        {
            const string propertyName = nameof(BusyIndicatorViewModel.IsIntermediate);

            void ChangeProperty(BusyIndicatorViewModel x)
            {
                x.IsIntermediate = true;
            }

            WhenChangeProperty_ThenRaisePropertyChanged(propertyName, ChangeProperty);
        }

        #endregion

        [Test]
        public void WhenPassNullAsScheduler_ThenDoesNotThrowException()
        {
            //Arrange

            //Act

            //Assert
            Assert.That(() =>
            {
                IScheduler scheduler = null;
                _busyIndicatorVM = new BusyIndicatorViewModel(TimeSpan.Zero, scheduler);
            }, Throws.Nothing);
        }

        [Test]
        public void WhenUseDefaultConstructor_BusyStateChangesImmediately()
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            //Act
            _busyIndicatorVM.SetBusyState(true);

            //Assert
            Assert.That(_busyIndicatorVM.IsBusy, Is.True);
        }

        private void WhenChangeProperty_ThenRaisePropertyChanged(string propertyName, Action<BusyIndicatorViewModel> changeProperty)
        {
            //Arrange
            _busyIndicatorVM = new BusyIndicatorViewModel();

            bool wasRaised = false;
            var subscription = _busyIndicatorVM.PropertyChangedObservable(propertyName)
                                               .Subscribe(_ => wasRaised = true);

            //Act
            changeProperty(_busyIndicatorVM);

            //Assert
            subscription.Dispose();
            Assert.That(wasRaised, Is.True);
        }
    }
}
