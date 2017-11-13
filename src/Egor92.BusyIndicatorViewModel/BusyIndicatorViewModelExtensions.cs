using System;
using System.Reactive;
using System.Reactive.Concurrency;

namespace Egor92
{
    public static class BusyIndicatorViewModelExtensions
    {
        public static void ExecuteBeingBusy(this BusyIndicatorViewModel @this, IScheduler scheduler, Action<Unit> action)
        {
            @this.IsBusy = true;

            action(Unit.Default);
        }
    }
}
