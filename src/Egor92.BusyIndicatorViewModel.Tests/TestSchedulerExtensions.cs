using System;
using Microsoft.Reactive.Testing;

namespace Egor92.Tests
{
    public static class TestSchedulerExtensions
    {
        public static void AdvanceBy(this TestScheduler testScheduler, TimeSpan time)
        {
            testScheduler.AdvanceBy(time.Ticks);
        }

        public static void AdvanceTo(this TestScheduler testScheduler, TimeSpan time)
        {
            testScheduler.AdvanceTo(time.Ticks);
        }
    }
}
