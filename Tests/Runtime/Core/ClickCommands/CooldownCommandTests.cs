using Agava.Merge2.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class CooldownCommandTests
    {
        private CooldownRepository _repository;
        private IClickCommand _command;

        [SetUp]
        public void Initialize()
        {
            var board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), new[]
            {
                new MapCoordinate(0, 0), new MapCoordinate(1, 0), new MapCoordinate(2, 0), new MapCoordinate(3, 0),
                new MapCoordinate(0, 1), new MapCoordinate(1, 1), new MapCoordinate(2, 1), new MapCoordinate(3, 1),
            });
            board.Add(new Item("Item1"), new MapCoordinate(1, 0));
            board.Add(new Item("Item2"), new MapCoordinate(2, 0));

            var settings = new CooldownSettings(new[]
            {
                new KeyValuePair<Item, Cooldown>(new Item("Item1"), new Cooldown(5, 2)),
                new KeyValuePair<Item, Cooldown>(new Item("Item2"), new Cooldown(10, 1)),
            });

            _repository = new CooldownRepository(settings, new LocalTimeProvider(), new MemoryJsonSaveRepository());
            _command = new CooldownCommand(board, _repository);
        }

        [Test]
        public void Execute()
        {
            _command.Execute(0, new MapCoordinate(1, 0));
            Assert.AreEqual(1, _repository.Items.Count);

            for (int i = 0; i < 4; i++)
                _command.Execute(0, new MapCoordinate(1, 0));

            Assert.Throws<CommandException>(() => _command.Execute(0, new MapCoordinate(1, 0)));
        }

        [Test]
        public void Undo()
        {
            Assert.Throws<InvalidOperationException>(() => _command.Undo());

            _command.Execute(0, new MapCoordinate(1, 0));
            Assert.DoesNotThrow(() => _command.Undo());
            Assert.AreEqual(0, _repository.Items.Count);
            Assert.Throws<InvalidOperationException>(() => _command.Undo());

            _command.Execute(0, new MapCoordinate(1, 0));
            _command.Execute(0, new MapCoordinate(1, 0));
            Assert.DoesNotThrow(() => _command.Undo());
            Assert.Throws<InvalidOperationException>(() => _command.Undo());
        }
    }
}
