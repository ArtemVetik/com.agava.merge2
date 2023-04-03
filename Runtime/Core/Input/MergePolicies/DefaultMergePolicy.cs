using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class DefaultMergePolicy : IMergePolicy
    {
        private readonly IBoard _board;
        private readonly Dictionary<string, int> _maxLevelByItemsId;

        public DefaultMergePolicy(IBoard board) : this(board, new Dictionary<string, int>()) { }
        
        public DefaultMergePolicy(IBoard board, IEnumerable<KeyValuePair<string, int>> maxLevelByItemsId)
        {
            _board = board;
            _maxLevelByItemsId = new Dictionary<string, int>(maxLevelByItemsId);
        }
        
        public bool CanMerge(MapCoordinate firstItemCoordinate, MapCoordinate secondItemCoordinate)
        {
            if (_board.Contains(firstItemCoordinate) == false || _board.Contains(secondItemCoordinate) == false)
                return false;
               
            if (firstItemCoordinate == secondItemCoordinate)
                return false;
            
            bool hasItems = _board.HasItem(firstItemCoordinate) && _board.HasItem(secondItemCoordinate);

            if (hasItems == false || ItemsLocked() || _board.Contour(firstItemCoordinate))
                return false;

            var firstItem = _board.Item(firstItemCoordinate);
            var secondItem = _board.Item(secondItemCoordinate);

            if (firstItem.Equals(secondItem) == false)
                return false;

            return ItemHasMaxLevel(firstItem) == false;

            bool ItemHasMaxLevel(Item item) => _maxLevelByItemsId.TryGetValue(item.Id, out int maxLevel) == false || item.Level == maxLevel;
            bool ItemsLocked() =>
                (_board.Opened(firstItemCoordinate) || _board.Contour(firstItemCoordinate)) == false ||
                (_board.Opened(secondItemCoordinate) || _board.Contour(secondItemCoordinate)) == false;
        }

        public void Merge(MapCoordinate firstItemCoordinate, MapCoordinate secondItemCoordinate)
        {
            if (CanMerge(firstItemCoordinate, secondItemCoordinate) == false)
                throw new InvalidOperationException("Can't merge");

            var item = _board.Item(firstItemCoordinate);
            
            _board.Remove(firstItemCoordinate);
            _board.Remove(secondItemCoordinate);
            
            _board.Add(item.Next(), secondItemCoordinate);
            
            if (_board.Opened(secondItemCoordinate) == false)
                _board.Open(secondItemCoordinate);
        }
    }
}