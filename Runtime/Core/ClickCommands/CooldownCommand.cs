using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class CooldownCommand : IClickCommand
    {
        private readonly IBoard _board;
        private readonly CooldownRepository _cooldownRepository;

        private Item _executedItem;

        public CooldownCommand(IBoard board, CooldownRepository cooldownRepository)
        {
            _board = board;
            _cooldownRepository = cooldownRepository;
            _executedItem = new Item("");
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            _executedItem = _board.Item(clickPosition);

            if (_cooldownRepository.Completed(_executedItem) == false)
                throw new CommandException("Cooldown not completed");

            _cooldownRepository.AddClick(_executedItem);
        }

        void IClickCommand.Undo()
        {
            if (_executedItem == new Item(""))
                throw new InvalidOperationException();

            _cooldownRepository.RemoveClick(_executedItem);
            _executedItem = new Item("");
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>();
        }
    }
}
