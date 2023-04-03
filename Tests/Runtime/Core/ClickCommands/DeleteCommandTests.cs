using System;
using System.Linq;
using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class DeleteCommandTests
    {
        private IBoard _board;
        private IClickCommand _deleteCommand;
        private MapCoordinate _itemPosition;

        [SetUp]
        public void Initialize()
        {
            _itemPosition = new MapCoordinate(5, 5);
            
            _board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), new MapCoordinate[] {_itemPosition});
            _deleteCommand = new DeleteCommand(_board);
            
            _board.Add(new Item("1"), _itemPosition);
        }

        [Test]
        public void ShouldDeleteItemAtPositionAndReturnItAfterUndo()
        {
            Assert.IsTrue(_board.HasItem(_itemPosition));

            Item item = _board.Item(_itemPosition);
            _deleteCommand.Execute(item.Level, _itemPosition);
            
            Assert.IsTrue(_board.HasItem(_itemPosition) == false);
            
            _deleteCommand.Undo();
            
            Assert.IsTrue(_board.Item(_itemPosition).Equals(item));
        }
        
        [Test]
        public void ShouldThrowExceptionIfCalledUndoTwoTimesAfterExecute()
        {
            Assert.IsTrue(_board.HasItem(_itemPosition));
            Assert.DoesNotThrow(() => _deleteCommand.Execute(_board.Item(_itemPosition).Level, _itemPosition));
            Assert.DoesNotThrow(() => _deleteCommand.Undo());
            Assert.Throws<InvalidOperationException>(() => _deleteCommand.Undo());
        }
        
        [Test]
        public void ShouldThrowExceptionIfCalledUndoBeforeExecute()
        {
            Assert.Throws<InvalidOperationException>(() => _deleteCommand.Undo());
        }

        [Test]
        public void ShouldThrowExceptionIfNoItemInPosition()
        {
            Assert.Throws<CommandException>(() => _deleteCommand.Execute(1, new MapCoordinate(1, 1)));
        }

        [Test]
        public void ShouldThrowExceptionIfNoPositionOnBoard()
        {
            Assert.Throws<CommandException>(() => _deleteCommand.Execute(1, new MapCoordinate(100, 100)));
        }

        [Test]
        public void ShouldCorrectFilter()
        {
            Assert.AreEqual(0, _deleteCommand.Filter<TestClickCommand>().Count());
            Assert.AreEqual(1, _deleteCommand.Filter<DeleteCommand>().Count());
        }
    }
}
