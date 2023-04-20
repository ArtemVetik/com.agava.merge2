using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public interface ITimeRepository
    {
        IReadOnlyCollection<string> Items { get; }
        TimeSpan Remains(Item item);
        bool Setting(Item item, out int seconds);
        bool Completed(Item item);
    }
}
