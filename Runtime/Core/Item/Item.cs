using System;
using Newtonsoft.Json;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// An immutable class for representing a game item.
    /// Two items are the same if they have the same <see cref="Id"/> and <see cref="Level"/>.
    /// Item cannot be null. Use new Item("") instead of checking for null.
    /// This class can be serialized in json by means of Newtonsoft.
    /// </summary>
    [Serializable]
    public class Item : IEquatable<Item>, ICloneable
    {
        /// <summary>
        /// The unique guid of the item.
        /// It is a distinctive property of identical items.
        /// </summary>
        [JsonProperty] public readonly string Guid;
        
        [JsonProperty] private readonly string _id;
        [JsonProperty] private readonly int _level;
        
        public Item(string id, int level = 0)
        {
            Guid = System.Guid.NewGuid().ToString();
            _id = id;
            _level = level;
        }

        [JsonIgnore] public string Id => _id;
        [JsonIgnore] public int Level => _level;

        /// <summary>
        /// Creates a new item that has the same id,
        /// but the level is one more than the current item.
        /// </summary>
        /// <returns></returns>
        public Item Next()
        {
            return new Item(Id, _level + 1);
        }

        public object Clone()
        {
            return new Item(_id, _level);
        }

        public bool Equals(Item other) => Id == other.Id && Level == other.Level;
        public override bool Equals(object obj) => obj is Item other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_id, _level);

        public static bool operator ==(Item a, Item b) => a.Equals(b);
        public static bool operator !=(Item a, Item b) => !(a == b);
    }
}
