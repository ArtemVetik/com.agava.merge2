using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class OpeningDelaySettings
    {
        private readonly Dictionary<Item, int> _settings;

        public OpeningDelaySettings(IEnumerable<KeyValuePair<Item, int>> settings)
        {
            _settings = new Dictionary<Item, int>(settings);
        }

        public bool Delay(Item item, out int delay)
        {
            return _settings.TryGetValue(item, out delay);
        }
    }
}
