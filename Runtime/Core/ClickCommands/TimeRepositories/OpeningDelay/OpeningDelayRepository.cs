using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class OpeningDelayRepository : ITimeRepository
    {
        private readonly OpeningDelaySettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IJsonSaveRepository _saveRepository;

        [JsonProperty] private readonly Dictionary<string, long> _delays;

        public OpeningDelayRepository(OpeningDelaySettings settings, ITimeProvider timeProvider, IJsonSaveRepository saveRepository)
        {
            _settings = settings ?? throw new ArgumentException();
            _timeProvider = timeProvider ?? throw new ArgumentException(); ;
            _saveRepository = saveRepository ?? throw new ArgumentException();

            _delays = new Dictionary<string, long>();
        }

        public IReadOnlyCollection<string> Items => _delays.Keys;

        public bool Setting(Item item, out int delay)
        {
            return _settings.Delay(item, out delay);
        }

        public TimeSpan Remains(Item item)
        {
            if (_settings.Delay(item, out int delay) == false)
                return TimeSpan.Zero;

            if (_delays.TryGetValue(item.Guid, out long startTime) == false)
                return new TimeSpan(0, 0, delay);

            var ellapsed = new TimeSpan(_timeProvider.NowTicks) - new TimeSpan(startTime);
            
            if (ellapsed.TotalSeconds >= delay)
                return TimeSpan.Zero;

            return new TimeSpan(0, 0, delay) - ellapsed;
        }

        public bool Completed(Item item)
        {
            if (_settings.Delay(item, out int delay) == false)
                return true;

            if (_delays.TryGetValue(item.Guid, out long startTime) == false)
                return false;

            var ellapsed = new TimeSpan(_timeProvider.NowTicks) - new TimeSpan(startTime);
            return ellapsed.TotalSeconds >= delay;
        }

        public void Open(Item item)
        {
            if (_settings.Delay(item, out _) == false)
                throw new InvalidOperationException();

            if (_delays.ContainsKey(item.Guid))
                throw new InvalidOperationException();

            _delays.Add(item.Guid, _timeProvider.NowTicks);

            var json = JsonConvert.SerializeObject(_delays);
            _saveRepository.Save(json);
        }

        public void Remove(Item item)
        {
            if (_delays.TryGetValue(item.Guid, out _) == false)
                throw new InvalidOperationException();
            
            _delays.Remove(item.Guid);
        }

        public void Load()
        {
            if (_saveRepository.HasSave == false)
                return;

            var json = _saveRepository.Load();
            var save = JsonConvert.DeserializeObject<Dictionary<string, long>>(json);

            _delays.Clear();

            foreach (var item in save)
                _delays.Add(item.Key, item.Value);
        }
    }
}
