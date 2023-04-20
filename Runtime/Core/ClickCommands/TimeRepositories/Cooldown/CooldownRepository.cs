using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class CooldownRepository : ITimeRepository
    {
        private readonly CooldownSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IJsonSaveRepository _saveRepository;
        private readonly Dictionary<string, ItemCooldown> _cooldownItems;

        public CooldownRepository(CooldownSettings settings, ITimeProvider timeProvider, IJsonSaveRepository saveRepository)
        {
            _settings = settings ?? throw new ArgumentNullException();
            _timeProvider = timeProvider ?? throw new ArgumentNullException();
            _saveRepository = saveRepository ?? throw new ArgumentNullException();

            _cooldownItems = new Dictionary<string, ItemCooldown>();
        }

        public IReadOnlyCollection<string> Items => _cooldownItems.Keys;
        public bool Cooldown(Item item, out Cooldown cooldown) => _settings.Cooldown(item, out cooldown);
        
        public bool Setting(Item item, out int seconds)
        {
            var hasSetting = Cooldown(item, out Cooldown cooldown);
            seconds = cooldown.ColldownSeconds;
            return hasSetting;
        }

        public TimeSpan Remains(Item item)
        {
            if (_settings.Cooldown(item, out Cooldown cooldown) == false)
                return TimeSpan.Zero;

            if (_cooldownItems.TryGetValue(item.Guid, out ItemCooldown itemCooldown) == false)
                return TimeSpan.Zero;

            var ellapsedTime = new TimeSpan(_timeProvider.NowTicks - itemCooldown.StartTime);

            if (ellapsedTime.TotalSeconds >= cooldown.ColldownSeconds)
                return TimeSpan.Zero;

            return new TimeSpan(0, 0, cooldown.ColldownSeconds) - ellapsedTime;
        }

        public bool Completed(Item item)
        {
            if (_settings.Cooldown(item, out Cooldown cooldown) == false)
                return true;

            if (_cooldownItems.TryGetValue(item.Guid, out ItemCooldown itemCooldown) == false)
                return true;

            return itemCooldown.ClickCount < cooldown.MaxClicks || Remains(item) == TimeSpan.Zero;
        }

        public void AddClick(Item item)
        {
            if (_settings.Cooldown(item, out Cooldown cooldown) == false)
                return;

            if (_cooldownItems.TryGetValue(item.Guid, out ItemCooldown itemCooldown) == false)
            {
                _cooldownItems.Add(item.Guid, new ItemCooldown(_timeProvider.NowTicks));
                Save();
                return;
            }

            if (itemCooldown.ClickCount < cooldown.MaxClicks)
            {
                _cooldownItems[item.Guid] = itemCooldown.Next();
                Save();
                return;
            }

            var elapsedTime = new TimeSpan(_timeProvider.NowTicks - itemCooldown.StartTime);
            if (elapsedTime.TotalSeconds >= cooldown.ColldownSeconds)
            {
                _cooldownItems[item.Guid] = new ItemCooldown(_timeProvider.NowTicks);
                Save();
                return;
            }

            throw new InvalidOperationException();
        }

        public void RemoveClick(Item item)
        {
            if (_cooldownItems.TryGetValue(item.Guid, out ItemCooldown itemCooldown) == false)
                throw new InvalidOperationException();

            if (itemCooldown.ClickCount > 1)
                _cooldownItems[item.Guid] = itemCooldown.Previous();
            else
                _cooldownItems.Remove(item.Guid);
        }

        public void Load()
        {
            if (_saveRepository.HasSave == false)
                return;

            var json = _saveRepository.Load();
            var savedDictionary = JsonConvert.DeserializeObject<Dictionary<string, ItemCooldown>>(json);

            _cooldownItems.Clear();

            foreach (var item in savedDictionary)
                _cooldownItems.Add(item.Key, item.Value);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_cooldownItems);
            _saveRepository.Save(json);
        }

        [Serializable]
        private struct ItemCooldown
        {
            [JsonProperty] public readonly long StartTime;

            [JsonProperty] private int _clickCount;

            public ItemCooldown(long startTime)
            {
                StartTime = startTime;
                _clickCount = 1;
            }

            private ItemCooldown(long startTime, int clickCount)
            {
                StartTime = startTime;
                _clickCount = clickCount;
            }

            [JsonIgnore] public int ClickCount => _clickCount;

            public ItemCooldown Next()
            {
                return new ItemCooldown(StartTime, _clickCount + 1);
            }

            public ItemCooldown Previous()
            {
                if (_clickCount <= 1)
                    throw new InvalidOperationException();

                return new ItemCooldown(StartTime, _clickCount - 1);
            }
        }
    }
}
