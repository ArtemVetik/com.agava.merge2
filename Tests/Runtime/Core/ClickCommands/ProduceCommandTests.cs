using NUnit.Framework;
using System;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class ProduceCommandTests
    {
        private IBoard _board;
        private IClickCommand _command;

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

            _command = new ProduceCommand(_board, new[]
            {
                new ProduceItems(new Item("00")),
                new ProduceItems(new Item("10"),new Item("11")),
                new ProduceItems(new Item("20"),new Item("21"),new Item("22")),
            });
        }

        [Test]
        public void ShouldAddCorrectItemToBoard()
        {
            _command.Execute(0, new MapCoordinate());

            Assert.True(_board.OpenedCollection
                .Where(coordinate => _board.HasItem(coordinate))
                .Count(coordinate => _board.Item(coordinate).Id == "00") == 1);

            _command.Execute(2, new MapCoordinate());

            Assert.True(_board.OpenedCollection
                .Where(coordinate => _board.HasItem(coordinate))
                .Any(coordinate => _board.Item(coordinate).Id == "20"
                                || _board.Item(coordinate).Id == "21"
                                || _board.Item(coordinate).Id == "22"));
        }

        [Test]
        public void ShouldThrowIfInvalidLevel()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _command.Execute(-1, new MapCoordinate()));
            Assert.Throws<IndexOutOfRangeException>(() => _command.Execute(5, new MapCoordinate()));
        }

        [Test]
        public void ShoulThrowWhenNoFreeCells()
        {
            for (int i = 0; i < 16; i++)
                _command.Execute(0, new MapCoordinate());

            Assert.Throws<CommandException>(() => _command.Execute(0, new MapCoordinate()));
        }

        [Test]
        public void ShouldThrowWhenUndoBeforeExecute()
        {
            Assert.Throws<InvalidOperationException>(_command.Undo);
        }

        [Test]
        public void ShouldRestoreBoardStateWhenUndo()
        {
            _command.Execute(0, new MapCoordinate());
            _command.Undo();

            Assert.True(_board.OpenedCollection
                .Where(coordinate => _board.HasItem(coordinate))
                .Count(coordinate => _board.Item(coordinate).Id == "00") == 0);
        }

        [Test]
        public void ShouldCorrectFilter()
        {
            Assert.AreEqual(0, _command.Filter<LimitCommand>().Count());
            Assert.AreEqual(0, _command.Filter<DeleteCommand>().Count());
            Assert.AreEqual(1, _command.Filter<ProduceCommand>().Count());
        }
    }
}
