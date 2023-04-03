using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Provides an interface for specifying
    /// a collection of items that a producer can produce.
    /// </summary>
    public interface IProduceItems
    {
        /// <summary>
        /// Collection of all items that can be produced.
        /// </summary>
        public IReadOnlyList<Item> Items { get; }

        /// <summary>
        /// Getting a random item from a collection.
        /// </summary>
        Item RandomItem();
    }
}
