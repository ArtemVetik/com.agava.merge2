using NUnit.Framework;
using Agava.Merge2.Tests;

namespace Agava.Merge2.Core.Tests.BoardTests
{
    public class JsonBoardSaveTests
    {
        [Test]
        public void ShouldSave()
        {
            var shape = new RectShape(10, 10);
            var contour = new RectContourAlgorithm();
            var board = new Board(shape, contour, new[]
            {
                new MapCoordinate(0, 0), new MapCoordinate(1, 0),
                new MapCoordinate(0, 1), new MapCoordinate(1, 1),
            });
            board.Add(new Item("Item1", 3), new MapCoordinate(0, 0));
            board.Add(new Item("Item2"), new MapCoordinate(3, 5));
            board.Add(new Item("Item3", 1), new MapCoordinate(2, 0));

            var jsonSave = new JsonBoardSave(new MemoryJsonSaveRepository());
            jsonSave.Save(board);

            var savedBoard = jsonSave.Load(shape, contour);

            Assert.AreEqual(4, savedBoard.OpenedCollection.Count);
            Assert.AreEqual(new Item("Item1", 3), savedBoard.Item(new MapCoordinate(0, 0)));
            Assert.AreEqual(new Item("Item2"), savedBoard.Item(new MapCoordinate(3, 5)));
            Assert.AreEqual(new Item("Item3", 1), savedBoard.Item(new MapCoordinate(2, 0)));
        }
    }
}
