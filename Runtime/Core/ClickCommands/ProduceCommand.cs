using System;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core
{
    public class ProduceCommand : IClickCommand
    {
        private readonly IBoard _board;
        private readonly IProduceItems[] _levels;
        private readonly Random _random;

        private MapCoordinate? _lastExecute;

        public ProduceCommand(IBoard board, IEnumerable<IProduceItems> levels)
        {
            _board = board;
            _levels = levels.ToArray();
            _random = new Random();
        }

        public IReadOnlyList<IProduceItems> Levels => _levels;

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            var freeCoordinates = _board.OpenedCollection.Where(coordinate => _board.HasItem(coordinate) == false).ToArray();

            if (freeCoordinates.Length == 0)
                throw new CommandException();

            var item = _levels[itemLevel].RandomItem();
            var coordinate = freeCoordinates[_random.Next(0, freeCoordinates.Length)];

            _board.Add(item, coordinate);
            _lastExecute = coordinate;
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>();
        }

        void IClickCommand.Undo()
        {
            if (_lastExecute == null)
                throw new InvalidOperationException();

            _board.Remove(_lastExecute.Value);
            _lastExecute = null;
        }
    }
}
