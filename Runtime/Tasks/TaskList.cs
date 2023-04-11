using Agava.Merge2.Core;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Tasks
{
    public class TaskList
    {
        private readonly IBoard _board;
        private readonly TaskReward _reward;
        private readonly List<Task> _tasks;
        private readonly TaskProgress _progressTask;

        public TaskList(IBoard board, IEnumerable<Task> tasks, TaskReward reward)
        {
            _board = board;
            _tasks = new List<Task>(tasks);
            _reward = reward;
            _progressTask = new TaskProgress(board);
        }

        public TaskList(IBoard board, TaskReward reward)
            : this(board, Array.Empty<Task>(), reward)
        { }

        public IReadOnlyCollection<Task> Tasks => _tasks;

        public void Add(Task task)
        {
            _tasks.Add(task);
        }

        public void Remove(Task task)
        {
            if (_tasks.Remove(task) == false)
                throw new InvalidOperationException();
        }

        public void Complete(Task task)
        {
            if (_tasks.Contains(task) == false)
                throw new InvalidOperationException("Task not found in task list");

            _progressTask.Compute(task);

            if (_progressTask.RequiredItems.Count != 0)
                throw new InvalidOperationException("Task can not be completed");

            _reward.Award(task);

            foreach (var position in _progressTask.ContainedPositions)
                _board.Remove(position);

            _tasks.Remove(task);
        }
    }
}
