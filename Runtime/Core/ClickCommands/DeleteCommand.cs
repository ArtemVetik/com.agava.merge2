using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class DeleteCommand : IClickCommand
    {
        private readonly IBoard _board;
        private bool _hasDeletedItem;
        private KeyValuePair<MapCoordinate, Item> _lastDeletedItemPair;

        public DeleteCommand(IBoard board)
        {
            _board = board;
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            if (_board.Contains(clickPosition) == false || _board.HasItem(clickPosition) == false)
                throw new CommandException(nameof(clickPosition));

            _hasDeletedItem = true;
            _lastDeletedItemPair = new KeyValuePair<MapCoordinate, Item>(clickPosition, _board.Item(clickPosition));
            
            _board.Remove(clickPosition);
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>();
        }

        void IClickCommand.Undo()
        {
            if (_hasDeletedItem == false)
                throw new InvalidOperationException("No deleted items");
            
            _board.Add(_lastDeletedItemPair.Value, _lastDeletedItemPair.Key);
            _hasDeletedItem = false;
        }
    }
}