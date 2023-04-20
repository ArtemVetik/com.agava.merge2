using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class CooldownSettings
    {
        private readonly IDictionary<Item, Cooldown> _settings;

        public CooldownSettings(IEnumerable<KeyValuePair<Item, Cooldown>> settings)
        {
            _settings = new Dictionary<Item, Cooldown>(settings);
        }

        public bool Cooldown(Item item, out Cooldown cooldown)
        {
            return _settings.TryGetValue(item, out cooldown);
        }
    }
}
