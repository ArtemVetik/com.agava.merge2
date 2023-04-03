using System;
using Agava.Merge2.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agava.Merge2.Tasks
{
    [Serializable]
    public class Task
    {
        [JsonProperty] private readonly Item[] _totalItems;

        [JsonConstructor]
        private Task() { }

        public Task(Item[] items)
        {
            if (items == null || items.Length == 0)
                throw new ArgumentException(nameof(items));

            _totalItems = items;
        }

        [JsonIgnore] public IReadOnlyCollection<Item> TotalItems => _totalItems;
    }
}
