using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class CommandQueue : IClickCommand
    {
        private readonly IEnumerable<IClickCommand> _commands;
        private readonly Stack<IClickCommand> _lastExecuted;

        public CommandQueue(IEnumerable<IClickCommand> commands)
        {
            _commands = commands;
            _lastExecuted = new Stack<IClickCommand>();
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            try
            {
                _lastExecuted.Clear();
                
                foreach (var command in _commands)
                {
                    command.Execute(itemLevel, clickPosition);
                    _lastExecuted.Push(command);
                }
            }
            catch (CommandException exception)
            {
                while (_lastExecuted.Count > 0)
                {
                    _lastExecuted.Pop().Undo();
                }

                throw exception;
            }
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>(_commands);
        }

        void IClickCommand.Undo()
        {
            if (_lastExecuted.Count == 0)
                throw new InvalidOperationException();

            while (_lastExecuted.Count > 0)
                _lastExecuted.Pop().Undo();
        }
    }
}
