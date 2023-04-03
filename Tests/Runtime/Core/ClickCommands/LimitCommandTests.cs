using System;
using System.Linq;
using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public partial class LimitCommandTests
    {
        private IBoard _board;
        private IClickCommand _limitCommand;
        private TestClickCommand _nestedClickCommand;

        [SetUp]
        public void Initialize()
        {
            _board = new Board(new RectShape(2, 2), new RectContourAlgorithm(), new[]
            {
                new MapCoordinate(0, 0), new MapCoordinate(0, 1),
                new MapCoordinate(1, 0), new MapCoordinate(1, 1)
            });
            
            _board.Add(new Item("100"), new MapCoordinate(0, 0));
            _board.Add(new Item("100"), new MapCoordinate(0, 1));
            _board.Add(new Item("001"), new MapCoordinate(1, 0));
            
            _nestedClickCommand = new TestClickCommand();
            _limitCommand = new LimitCommand(2, _board, _nestedClickCommand);
        }

        [Test]
        public void ShouldNoCallExecuteNestedClickCommandBeforeLimit()
        {
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            Assert.IsFalse(_nestedClickCommand.ExecuteCalled);
        }

        [Test]
        public void ShouldCallExecuteNestedClickCommandAfterLimit()
        {
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            Assert.IsTrue(_nestedClickCommand.ExecuteCalled);
        }

        [Test]
        public void ShouldCallExecuteTwoTimesNestedClickCommandTwiceAfterLimitTwice()
        {
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            Assert.IsTrue(_nestedClickCommand.ExecuteCalled);
            _nestedClickCommand.Clear();
            Assert.IsFalse(_nestedClickCommand.ExecuteCalled);
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            Assert.IsTrue(_nestedClickCommand.ExecuteCalled);
        }

        [Test]
        public void ShouldNoThrowIfCallUndoNestedClickCommandAfterExecute()
        {
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            _limitCommand.Execute(1, new MapCoordinate(0, 0));
            Assert.IsTrue(_nestedClickCommand.ExecuteCalled);
            _limitCommand.Undo();
            Assert.IsFalse(_nestedClickCommand.ExecuteCalled);
        }

        [Test]
        public void ShouldThrowIfCalledUndoTwoTimesAfterExecute()
        {
            Assert.DoesNotThrow(() => _limitCommand.Execute(1, new MapCoordinate(0, 0)));
            Assert.DoesNotThrow(() => _limitCommand.Undo());
            Assert.Throws<InvalidOperationException>(() => _limitCommand.Undo());
        }

        [Test]
        public void ShouldThrowIfCalledUndoBeforeExecute()
        {
            Assert.Throws<InvalidOperationException>(() => _limitCommand.Undo());
        }
        
        [Test]
        public void ExecuteShouldThrowIfLimitLessThanZero()
        {
            IClickCommand limitCommand = new LimitCommand(-1, _board, _nestedClickCommand);
            Assert.Throws<CommandException>(() => limitCommand.Execute(1, new MapCoordinate(1, 0)));
        }
        
        [Test]
        public void ShouldThrowIfNoItemOnBoardAtClickPosition()
        {
            Assert.Throws<CommandException>(() => _limitCommand.Execute(0, new MapCoordinate(1, 1)));
        }
        
        [Test]
        public void ShouldNoCallExecuteNestedClickCommandIfCalledExecuteLimitNumberOfTimesForDifferentPositions()
        {
            _limitCommand.Execute(0, new MapCoordinate(1, 0));
            _limitCommand.Execute(0, new MapCoordinate(0, 1));
            Assert.IsFalse(_nestedClickCommand.ExecuteCalled);
        }

        [Test]
        public void ShouldCorrectFilter()
        {
            Assert.AreEqual(0, _limitCommand.Filter<DeleteCommand>().Count());
            Assert.AreEqual(1, _limitCommand.Filter<LimitCommand>().Count());
            Assert.AreEqual(1, _limitCommand.Filter<TestClickCommand>().Count());
        }
    }
}