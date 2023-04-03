using System.Linq;
using System.Collections.Generic;
using System;

namespace Agava.Merge2.Core
{
    public class ClickPolicy
    {
        private readonly Dictionary<string, IClickCommand> _clickItems;
        private readonly IBoard _board;

        public ClickPolicy(IBoard board, params (string, IClickCommand)[] clickItems) 
            : this(board, clickItems.ToDictionary(item =>
                    item.Item1, item => item.Item2)) 
        { }

        public ClickPolicy(IBoard board, Dictionary<string, IClickCommand> clickItems)
        {
            _board = board;
            _clickItems = clickItems;
        }

        public void Click(MapCoordinate coordinate)
        {
            if (_board.HasItem(coordinate) == false)
                throw new InvalidOperationException("Click on an empty cell");

            var item = _board.Item(coordinate);

            if (_clickItems.ContainsKey(item.Id) == false)
                throw new InvalidOperationException("Click command not found");

            _clickItems[item.Id].Execute(item.Level, coordinate);
        }
    }
}
