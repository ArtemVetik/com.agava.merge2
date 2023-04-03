using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Default implementation of <see cref="IProduceItems"/>.
    /// All items have the same chance of getting from the <see cref="RandomItem"/> method.
    /// </summary>
    public class ProduceItems : IProduceItems
    {
        private readonly List<Item> _items;
        private readonly Random _random;

        public ProduceItems(params Item[] items)
        {
            _items = new List<Item>(items);
            _random = new Random();
        }

        public IReadOnlyList<Item> Items => _items;

        public Item RandomItem()
        {
            return _items[_random.Next(0, _items.Count)];
        }
    }
}
