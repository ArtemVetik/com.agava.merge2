using System;

namespace Agava.Merge2.Core
{
    public class DefaultMovePolicy : IMovePolicy
    {
        private readonly IBoard _board;
        
        public DefaultMovePolicy(IBoard board)
        {
            _board = board;
        }

        public bool CanMove(MapCoordinate fromPosition, MapCoordinate toPosition)
        {
            if (_board.Contains(fromPosition) == false || _board.Contains(toPosition) == false)
                return false;
            
            if (fromPosition == toPosition)
                return true;

            if (_board.HasItem(fromPosition) == false || PositionOpened(fromPosition) == false)
                return false;

            if (PositionOpened(toPosition) == false)
                return false;
            
            if (_board.HasItem(toPosition) == false)
                return true;

            var firstItem = _board.Item(fromPosition);
            var secondItem = _board.Item(toPosition);
            
            return firstItem.Equals(secondItem) == false;

            bool PositionOpened(MapCoordinate position) => _board.Opened(position) && _board.Contour(position) == false;
        }

        public void Move(MapCoordinate fromPosition, MapCoordinate toPosition)
        {
            if (CanMove(fromPosition, toPosition) == false)
                throw new InvalidOperationException("Can't move");

            var fromItem = _board.Item(fromPosition);
            _board.Remove(fromPosition);

            if (_board.HasItem(toPosition))
            {
                var toItem = _board.Item(toPosition);
                _board.Remove(toPosition);
                
                _board.Add(toItem, fromPosition);
            }
            
            _board.Add(fromItem, toPosition);
        }
    }
}