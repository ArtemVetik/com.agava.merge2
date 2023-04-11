using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core
{
    public class CommandFilter
    {
        private readonly Dictionary<string, IClickCommand> _clickCommands;

        public CommandFilter(IEnumerable<KeyValuePair<string, IClickCommand>> clickCommands)
        {
            _clickCommands = new Dictionary<string, IClickCommand>(clickCommands);
            FilteredId = _clickCommands.Select(command => command.Key);
        }

        public IEnumerable<string> FilteredId { get; private set; }

        public IEnumerable<KeyValuePair<string, IEnumerable<T>>> Filter<T>() where T : IClickCommand
        {
            var result = new Dictionary<string, IEnumerable<T>>();

            foreach (var item in _clickCommands)
            {
                var filtered = item.Value.Filter<T>();

                if (filtered.Count() != 0)
                    result.Add(item.Key, filtered);
            }

            return result;
        }
    }
}
