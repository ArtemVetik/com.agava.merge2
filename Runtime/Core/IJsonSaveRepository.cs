
namespace Agava.Merge2.Core
{
    /// <summary>
    /// Provides an interface for creating a repository for storing json strings.
    /// </summary>
    public interface IJsonSaveRepository
    {
        bool HasSave { get; }
        void Save(string json);
        string Load();
    }
}
