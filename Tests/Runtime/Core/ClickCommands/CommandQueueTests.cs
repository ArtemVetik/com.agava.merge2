using NUnit.Framework;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class CommandQueueTests
    {
        private IBoard _board;

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
        }

        [Test]
        public void ShouldExecuteAllCommands()
        {
            _board.Add(new Item("producer"), new MapCoordinate(2, 3));
            IClickCommand command = new CommandQueue(new IClickCommand[]
            {
                new ProduceCommand(_board, new[] { new ProduceItems(new Item("0"))} ),
                new ProduceCommand(_board, new[] { new ProduceItems(new Item("0"))} ),
                new DeleteCommand(_board),
            });

            Assert.DoesNotThrow(() => command.Execute(0, new MapCoordinate(2, 3)));
        }

        [Test]
        public void ShouldThrowAndUndoAllCommands()
        {
            _board.Add(new Item("test"), new MapCoordinate(1, 1));
            IClickCommand command = new CommandQueue(new[]
            {
                new DeleteCommand(_board),
                new DeleteCommand(_board),
            });

            Assert.Throws<CommandException>(() => command.Execute(0, new MapCoordinate(1, 1)));
            Assert.That(_board.HasItem(new MapCoordinate(1, 1)));
        }

        [Test]
        public void ShouldCorrectUndoAfterExecute()
        {
            _board.Add(new Item("producer"), new MapCoordinate(0, 0));

            IClickCommand command = new CommandQueue(new IClickCommand[]
            {
                new ProduceCommand(_board, new[] { new ProduceItems(new Item("0"))} ),
                new ProduceCommand(_board, new[] { new ProduceItems(new Item("0"))} ),
                new DeleteCommand(_board),
            });

            command.Execute(0, new MapCoordinate(0, 0));

            Assert.That(_board.HasItem(new MapCoordinate(0, 0)) == false);
            Assert.That(_board.OpenedCollection.Count(coordinate =>
                                    _board.HasItem(coordinate) && _board.Item(coordinate).Id == "0") == 2);

            command.Undo();

            Assert.That(_board.HasItem(new MapCoordinate(0, 0)) == true);
            Assert.That(_board.OpenedCollection.Count(coordinate =>
                                    _board.HasItem(coordinate) && _board.Item(coordinate).Id == "0") == 0);
        }

        [Test]
        public void ShouldCorrectFilter()
        {
            IClickCommand command = new CommandQueue(new IClickCommand[]
            {
                new DeleteCommand(_board),
                new CommandQueue(new IClickCommand[]
                {
                    new DeleteCommand(_board),
                    new TestClickCommand("Test"),
                }),
            });

            Assert.AreEqual(0, command.Filter<ProduceCommand>().Count());
            Assert.AreEqual(1, command.Filter<TestClickCommand>().Count());
            Assert.AreEqual(2, command.Filter<DeleteCommand>().Count());
        }
    }
}
