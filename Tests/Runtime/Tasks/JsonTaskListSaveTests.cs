using Agava.Merge2.Core;
using Agava.Merge2.Tests;
using NUnit.Framework;
using System.Linq;

namespace Agava.Merge2.Tasks.Tests
{
    public class JsonTaskListSaveTests
    {
        private IBoard _board;
        private TaskList _taskList;
        private IJsonSaveRepository _saveRepository;

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

            _taskList = new TaskList(_board);
            _saveRepository = new MemoryJsonSaveRepository();
        }

        [Test]
        public void ShouldLoad()
        {
            var save = new JsonTaskListSave(_saveRepository);
            _taskList.Add(new Task(new[] { new Item("Item1") }));
            _taskList.Add(new Task(new[] { new Item("Item2"), new Item("Item3") }));
            
            save.Save(_taskList);

            var savedList = save.Load(_board);

            Assert.AreEqual(2, savedList.Tasks.Count);
            Assert.AreEqual(1, savedList.Tasks.ToArray()[0].TotalItems.Count);
            Assert.AreEqual(2, savedList.Tasks.ToArray()[1].TotalItems.Count);

            Assert.AreEqual("Item1", savedList.Tasks.ToArray()[0].TotalItems.ToArray()[0].Id);
            Assert.AreEqual("Item2", savedList.Tasks.ToArray()[1].TotalItems.ToArray()[0].Id);
            Assert.AreEqual("Item3", savedList.Tasks.ToArray()[1].TotalItems.ToArray()[1].Id);
        }
    }
}
