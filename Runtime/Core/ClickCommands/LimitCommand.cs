using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class LimitCommand : IClickCommand
    {
        private readonly int _limit;
        private readonly IBoard _board;
        private readonly IClickCommand _clickCommand;
        private readonly IJsonSaveRepository _saveRepository;
        private readonly Dictionary<string, ExecutesPair> _executesPairByItemGuid;
        private string _lastExecutedItemGuid;

        public LimitCommand(int limit, IBoard board, IClickCommand clickCommand, IJsonSaveRepository saveRepository)
        {
            _limit = limit;
            _board = board;
            _clickCommand = clickCommand;
            _saveRepository = saveRepository;
            _lastExecutedItemGuid = "";

            if (_saveRepository.HasSave)
            {
                var json = _saveRepository.Load();
                _executesPairByItemGuid = JsonConvert.DeserializeObject<Dictionary<string, ExecutesPair>>(json);
            }
            else
            {
                _executesPairByItemGuid = new Dictionary<string, ExecutesPair>();
            }
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            if (_limit < 0)
                throw new CommandException("Limit less than 0");

            if (_board.HasItem(clickPosition) == false)
                throw new CommandException(nameof(clickPosition));

            _lastExecutedItemGuid = _board.Item(clickPosition).Guid;

            if (_executesPairByItemGuid.ContainsKey(_lastExecutedItemGuid) == false)
                _executesPairByItemGuid.Add(_lastExecutedItemGuid, new ExecutesPair());

            var clickItemExecutesPair = _executesPairByItemGuid[_lastExecutedItemGuid];
            clickItemExecutesPair.ExecutesCount += 1;

            _saveRepository.Save(JsonConvert.SerializeObject(_executesPairByItemGuid));

            if (clickItemExecutesPair.ExecutesCount != _limit)
                return;

            clickItemExecutesPair.LimitHasBeenReached = true;
            _clickCommand.Execute(itemLevel, clickPosition);

            clickItemExecutesPair.ExecutesCount = 0;
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>(new[] { _clickCommand });
        }

        void IClickCommand.Undo()
        {
            if (_executesPairByItemGuid.ContainsKey(_lastExecutedItemGuid) == false)
                throw new InvalidOperationException();

            var executesPair = _executesPairByItemGuid[_lastExecutedItemGuid];

            if (executesPair.ExecutesCount == 0 && executesPair.LimitHasBeenReached == false)
                throw new InvalidOperationException();

            _lastExecutedItemGuid = "";

            if (executesPair.LimitHasBeenReached)
            {
                if (executesPair.ExecutesCount == 0)
                {
                    executesPair.LimitHasBeenReached = false;
                    executesPair.ExecutesCount = _limit - 1;
                    _clickCommand.Undo();
                    _saveRepository.Save(JsonConvert.SerializeObject(_executesPairByItemGuid));
                    return;
                }

                executesPair.ExecutesCount -= 1;
                _saveRepository.Save(JsonConvert.SerializeObject(_executesPairByItemGuid));
            }
            else
            {
                if (executesPair.ExecutesCount == 0)
                    throw new InvalidOperationException();

                executesPair.ExecutesCount -= 1;
                _saveRepository.Save(JsonConvert.SerializeObject(_executesPairByItemGuid));
            }
        }

        private class ExecutesPair
        {
            public int ExecutesCount;
            public bool LimitHasBeenReached;
        }
    }
}