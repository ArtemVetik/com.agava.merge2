using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// It is an arbitrary form.
    /// Note: width and height are calculated as the maximum
    /// value of the x and y coordinates
    /// </summary>
    public class ArbitaryShape : IShape
    {
        private readonly HashSet<MapCoordinate> _coordinates;
        private readonly int _width;
        private readonly int _height;

        public ArbitaryShape(IEnumerable<MapCoordinate> shapeCoordinates)
        {
            _coordinates = new HashSet<MapCoordinate>(shapeCoordinates);
            ComputeWidthHeight(shapeCoordinates, out _width, out _height);
        }

        public int Width => _width;
        public int Height => _height;

        public bool Contains(MapCoordinate position)
        {
            return _coordinates.Contains(position);
        }

        public bool IsSupersetOf(IEnumerable<MapCoordinate> other)
        {
            return _coordinates.IsSupersetOf(other);
        }

        private void ComputeWidthHeight(IEnumerable<MapCoordinate> coordinates, out int width, out int height)
        {
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (var coordinate in coordinates)
            {
                if (coordinate.X > maxWidth)
                    maxWidth = coordinate.X;
                if (coordinate.Y > maxHeight)
                    maxHeight = coordinate.Y;
            }

            width = maxWidth + 1;
            height = maxHeight + 1;
        }
    }
}
