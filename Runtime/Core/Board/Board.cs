using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class Board : IBoard
    {
        private readonly IShape _shape;
        private readonly IContourAlgorithm _contourAlgorithm;
        private readonly Dictionary<MapCoordinate, Item> _items;
        private readonly HashSet<MapCoordinate> _opened;
        private readonly HashSet<MapCoordinate> _contour;

        public Board(IShape shape, IContourAlgorithm contourAlgorithm, IEnumerable<MapCoordinate> opened)
        {
            if (shape.IsSupersetOf(opened) == false)
                throw new ArgumentException(nameof(opened));

            _shape = shape;
            _contourAlgorithm = contourAlgorithm;

            _opened = new HashSet<MapCoordinate>(opened);
            _items = new Dictionary<MapCoordinate, Item>();
            _contour =  new HashSet<MapCoordinate>(_contourAlgorithm.Compute(_opened));
        }

        public event Action Updated;
        
        public IReadOnlyCollection<MapCoordinate> OpenedCollection => _opened;
        public IReadOnlyCollection<MapCoordinate> ContourCollection => _contour;
        public int Width => _shape.Width;
        public int Height => _shape.Height;

        public void Add(Item item, MapCoordinate coordinate)
        {
            if (Contains(coordinate) == false)
                throw new InvalidOperationException();

            if (_items.TryAdd(coordinate, item) == false)
                throw new InvalidOperationException();
            
            Updated?.Invoke();
        }

        public void Remove(MapCoordinate coordinate)
        {
            if (_items.Remove(coordinate) == false)
                throw new InvalidOperationException();
            
            Updated?.Invoke();
        }

        public void Open(MapCoordinate coordinate)
        {
            if (_opened.Contains(coordinate))
                throw new InvalidOperationException();

            _opened.Add(coordinate);

            var contourCollection = _contourAlgorithm.Compute(_opened);
            _contour.Clear();
            
            foreach (var contourCoordinate in contourCollection)
                _contour.Add(contourCoordinate);
            
            Updated?.Invoke();
        }

        public Item Item(MapCoordinate coordinate)
        {
            if (_items.TryGetValue(coordinate, out Item value) == false)
                throw new InvalidOperationException();

            return value;
        }

        public bool HasItem(MapCoordinate coordinate)
        {
            return _items.ContainsKey(coordinate);
        }

        public bool Opened(MapCoordinate coordinate)
        {
            return _opened.Contains(coordinate);
        }

        public bool Contour(MapCoordinate coordinate)
        {
            return _contour.Contains(coordinate);
        }

        public bool Contains(MapCoordinate position)
        {
            return _shape.Contains(position);
        }

        public bool IsSupersetOf(IEnumerable<MapCoordinate> other)
        {
            return _shape.IsSupersetOf(other);
        }
    }
}
