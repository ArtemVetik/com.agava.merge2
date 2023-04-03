using System;
using Newtonsoft.Json;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// A structure for representing two-dimensional coordinates of the playing field.
    /// Note: <see cref="X"/> and <see cref="Y"/> cannot be negative.
    /// This struct can be serialized in json by means of Newtonsoft.
    /// </summary>
    [Serializable]
    public struct MapCoordinate : IEquatable<MapCoordinate>
    {
        [JsonProperty] public readonly int X;
        [JsonProperty] public readonly int Y;

        public MapCoordinate(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new ArgumentOutOfRangeException();

            X = x;
            Y = y;
        }

        public static MapCoordinate operator +(MapCoordinate a, MapCoordinate b)
            => new MapCoordinate(a.X + b.X, a.Y + b.Y);
        public static MapCoordinate operator -(MapCoordinate a, MapCoordinate b)
            => new MapCoordinate(a.X - b.X, a.Y - b.Y);
        public static bool operator ==(MapCoordinate a, MapCoordinate b)
            => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(MapCoordinate a, MapCoordinate b)
            => !(a == b);

        public bool Equals(MapCoordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is MapCoordinate mapPosition &&
                this == mapPosition;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}; {Y})";
        }
    }
}
