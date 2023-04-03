using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Rectangular shape of any positive width and height.
    /// </summary>
    public class RectShape : IShape
    {
        private readonly int _width;
        private readonly int _height;

        public RectShape(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            _width = width;
            _height = height;
        }

        public int Width => _width;
        public int Height => _height;

        public bool Contains(MapCoordinate position)
        {
            return position.X < _width && position.Y < _height;
        }

        public bool IsSupersetOf(IEnumerable<MapCoordinate> other)
        {
            foreach (var coordinate in other)
                if (Contains(coordinate) == false)
                    return false;

            return true;
        }
    }
}
