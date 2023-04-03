using Agava.Merge2.Core;
using System.Collections.Generic;

namespace Agava.Merge2.Tasks
{
    public class TaskProgress
    {
        private readonly IBoard _board;
        private readonly List<MapCoordinate> _containedPositions;
        private readonly List<Item> _requiredItems;

        public TaskProgress(IBoard board)
        {
            _board = board;
            _containedPositions = new List<MapCoordinate>();
            _requiredItems = new List<Item>();
        }

        public IReadOnlyCollection<MapCoordinate> ContainedPositions => _containedPositions;
        public IReadOnlyCollection<Item> RequiredItems => _requiredItems;

        public void Compute(Task task)
        {
            _containedPositions.Clear();
            _requiredItems.Clear();
            _requiredItems.AddRange(task.TotalItems);

            foreach (var position in _board.OpenedCollection)
                if (_board.HasItem(position))
                    if (_requiredItems.Remove(_board.Item(position)))
                        _containedPositions.Add(position);
        }
    }
}
