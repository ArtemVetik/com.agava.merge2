using Agava.Merge2.Core;
using NUnit.Framework;
using System.Linq;

namespace Agava.Merge2.Tasks.Tests
{
    public class TaskProgressTests
    {
        private IBoard _board;
        private TaskProgress _taskProgress;

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

            _taskProgress = new TaskProgress(_board);
        }

        [Test]
        public void ShouldCorrectCompute()
        {
            var task = new Task(new[] { new Item("Item2") });
            _taskProgress.Compute(task);

            Assert.AreEqual(0, _taskProgress.RequiredItems.Count);
            Assert.AreEqual(1, _taskProgress.ContainedPositions.Count);
            Assert.AreEqual(new MapCoordinate(1, 1), _taskProgress.ContainedPositions.ToArray()[0]);

            task = new Task(new[] { new Item("Item4"), new Item("Item2") });
            _taskProgress.Compute(task);

            Assert.AreEqual(0, _taskProgress.RequiredItems.Count);
            Assert.AreEqual(2, _taskProgress.ContainedPositions.Count);

            task = new Task(new[] { new Item("Item"), new Item("Item5"), new Item("null") });
            _taskProgress.Compute(task);

            Assert.AreEqual(3, _taskProgress.RequiredItems.Count);
            Assert.AreEqual(0, _taskProgress.ContainedPositions.Count);
        }
    }
}
