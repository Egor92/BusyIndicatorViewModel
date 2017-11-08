using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Egor92.Annotations;

namespace Egor92
{
    public sealed class BusyIndicatorController : INotifyPropertyChanged, IDisposable
    {
        #region Fields

        private readonly TimeSpan _delayTime;
        private readonly IScheduler _scheduler;
        private readonly ISubject<bool> _stateChangedSubject = new Subject<bool>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion

        #region Ctor

        public BusyIndicatorController(TimeSpan delayTime, IScheduler scheduler)
        {
            _delayTime = delayTime;
            _scheduler = scheduler;

            Initialize();
        }

        private void Initialize()
        {
            var isBusyUpdateSubscription = SubscribeToIsBusyUpdate();
            _disposables.Add(isBusyUpdateSubscription);
        }

        #endregion

        #region Properties

        #region IsBusy

        public bool IsBusy { get; private set; }

        private IDisposable SubscribeToIsBusyUpdate()
        {
            return _stateChangedSubject.Throttle(_delayTime, _scheduler)
                                       .Subscribe(isBusy => IsBusy = isBusy);
        }

        #endregion

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposables.Dispose();
        }

        #endregion

        #region Implementation of INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public void SetBusy()
        {
            _stateChangedSubject.OnNext(true);
        }
    }
}
