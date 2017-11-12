using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Egor92.Tests
{
    public static class ReactiveExensions
    {
        public static IObservable<EventPattern<PropertyChangedEventArgs>> PropertyChangedObservable(
            this INotifyPropertyChanged @this,
            string propertyName)
        {
            var whenPropertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(x =>
            {
                return x.Invoke;
            }, h =>
            {
                @this.PropertyChanged += h;
            }, h =>
            {
                @this.PropertyChanged -= h;
            });

            return whenPropertyChanged.Where(x => x.EventArgs.PropertyName == propertyName);
        }
    }
}
