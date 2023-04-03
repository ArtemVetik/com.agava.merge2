using System;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core
{
    public class OpenedItemList : IDisposable
    {
        private readonly IReadOnlyBoard _board;
        private readonly List<Item> _openedItems;
        
        internal OpenedItemList(IReadOnlyBoard board) : this(board, Array.Empty<Item>()) { }

        internal OpenedItemList(IReadOnlyBoard board, IEnumerable<Item> openedItems)
        {
            _board = board;
            _openedItems = new List<Item>(openedItems);
            
            _board.Updated += UpdateOpenedItems;
            UpdateOpenedItems();
        }

        public IEnumerable<Item> OpenedItems => _openedItems;

        public void Dispose()
        {
            _board.Updated -= UpdateOpenedItems;
        }
        
        private void UpdateOpenedItems()
        {
            var openedAndContourPositions = _board.OpenedCollection.Concat(_board.ContourCollection);

            foreach (var position in openedAndContourPositions)
            {
                if (_board.HasItem(position) == false)
                    continue;
                
                Item itemInPosition = _board.Item(position);
                
                if (_openedItems.Any(item => item == itemInPosition) == false)
                    _openedItems.Add(itemInPosition);
            }
        }
    }
}
