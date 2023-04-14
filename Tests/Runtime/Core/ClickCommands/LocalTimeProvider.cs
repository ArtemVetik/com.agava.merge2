using System;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class LocalTimeProvider : ITimeProvider
    {
        public long NowTicks => DateTime.Now.Ticks;
    }
}
