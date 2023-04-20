using System;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core
{
    public class OpeningDelayCommand : IClickCommand
    {
        private readonly IBoard _board;
        private readonly OpeningDelayRepository _repository;

        private Item _executedItem;

        public OpeningDelayCommand(IBoard board, OpeningDelayRepository repository)
        {
            _board = board;
            _repository = repository;
            _executedItem = new Item("");
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            _executedItem = _board.Item(clickPosition);

            if (_repository.Items.Contains(_executedItem.Guid) == false)
                _repository.Open(_executedItem);

            if (_repository.Completed(_executedItem) == false)
                throw new CommandException();
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>();
        }

        void IClickCommand.Undo()
        {
            if (_executedItem == new Item(""))
                throw new InvalidOperationException();

            _repository.Remove(_executedItem);
            _executedItem = new Item("");
        }
    }
}
