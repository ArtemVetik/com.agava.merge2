using Agava.Merge2.Core;

namespace Agava.Merge2.Tests
{
    public class MemoryJsonSaveRepository : IJsonSaveRepository
    {
        private string _savedValue = null;

        public bool HasSave => _savedValue != null;

        public string Load()
        {
            return _savedValue;
        }

        public void Save(string json)
        {
            _savedValue = json;
        }
    }
}
