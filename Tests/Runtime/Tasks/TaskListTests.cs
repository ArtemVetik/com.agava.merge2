using Agava.Merge2.Core;
using NUnit.Framework;
using System;
using System.Linq;

namespace Agava.Merge2.Tasks.Tests
{
    public class TaskListTests
    {
        private IBoard _board;
        private TaskList _taskList;
        private TestCurrency _testCurrency;

        [SetUp]
        public void Initialize()
        {
            _board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), new[]
            {
                new MapCoordinate(0, 0), new MapCoordinate(1, 0), new MapCoordinate(2, 0), new MapCoordinate(3, 0),
                 new MapCoordinate(0, 1), new MapCoordinate(1, 1), new MapCoordinate(2, 1), new MapCoordinate(3, 1),
                 new MapCoordinate(0, 2), new MapCoordinate(1, 2), new MapCoordinate(2, 2), new MapCoordinate(3, 2),
                 new MapCoordinate(0, 3), new MapCoordinate(1, 3), new MapCoordinate(2, 3), new MapCoordinate(3, 3),
            });

            _board.Add(new Item("Item1"), new MapCoordinate(0, 0));
            _board.Add(new Item("Item2"), new MapCoordinate(1, 1));
            _board.Add(new Item("Item3"), new MapCoordinate(2, 1));
            _board.Add(new Item("Item4"), new MapCoordinate(3, 2));
            _board.Add(new Item("Item5", 2), new MapCoordinate(2, 3));
            _board.Add(new Item("Item6", 1), new MapCoordinate(3, 3));

            _testCurrency = new TestCurrency();
            _taskList = new TaskList(_board, new TaskReward(_testCurrency, new TestRewardValue()));
        }

        [Test]
        public void ShouldAdd()
        {
            Assert.AreEqual(0, _taskList.Tasks.Count);
            _taskList.Add(new Task(new[] { new Item("Item") }));
            Assert.AreEqual(1, _taskList.Tasks.Count);
        }

        [Test]
        public void ShouldRemove()
        {
            var task1 = new Task(new[] { new Item("Item1") });
            var task2 = new Task(new[] { new Item("Item2") });

            _taskList.Add(task1);
            _taskList.Add(task2);

            _taskList.Remove(task1);
            Assert.AreEqual(1, _taskList.Tasks.Count);
            Assert.Throws<InvalidOperationException>(() => _taskList.Remove(task1));
        }

        [Test]
        public void ShouldCompleteAndRemoveItemAndTask()
        {
            var task = new Task(new[] { new Item("Item1"), new Item("Item3") });

            _taskList.Add(task);
            _taskList.Complete(task);

            Assert.False(_board.OpenedCollection.Any(coordinate => _board.HasItem(coordinate) && _board.Item(coordinate).Id == "Item1"));
            Assert.False(_board.OpenedCollection.Any(coordinate => _board.HasItem(coordinate) && _board.Item(coordinate).Id == "Item3"));
            Assert.AreEqual(0, _taskList.Tasks.Count);
        }

        [Test]
        public void ShouldAwardRewardAfterComplete()
        {
            var task = new Task(new[] { new Item("Item5", 2), new Item("Item3"), new Item("Item6", 1) });

            _taskList.Add(task);
            _taskList.Complete(task);

            Assert.AreEqual(6, _testCurrency.Value);
        }

        [Test]
        public void ShouldThrowIfCanNotComplete()
        {
            var task = new Task(new[] { new Item("Item11")});

            Assert.Throws<InvalidOperationException>(() => _taskList.Complete(task));

            _taskList.Add(task);
            Assert.Throws<InvalidOperationException>(() => _taskList.Complete(task));
        }
    }
}
