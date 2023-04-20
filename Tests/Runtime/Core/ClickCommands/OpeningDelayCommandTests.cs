using NUnit.Framework;
using Agava.Merge2.Tests;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine;
using System;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class OpeningDelayCommandTests
    {
        private IBoard _board;
        private IClickCommand _command;
        private OpeningDelayRepository _repository;

        [SetUp]
        public void Initialize()
        {
            _board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), new[]
            {
                new MapCoordinate(0, 0), new MapCoordinate(1, 0), new MapCoordinate(2, 0), new MapCoordinate(3, 0),
                new MapCoordinate(0, 1), new MapCoordinate(1, 1), new MapCoordinate(2, 1), new MapCoordinate(3, 1),
            });
            _board.Add(new Item("Item1"), new MapCoordinate(1, 0));
            _board.Add(new Item("Item2"), new MapCoordinate(2, 0));
            _board.Add(new Item("Item3"), new MapCoordinate(3, 0));

            var settings = new OpeningDelaySettings(new[]
            {
                new KeyValuePair<Item, int>(new Item("Item1"), 1),
                new KeyValuePair<Item, int>(new Item("Item2"), 2),
                new KeyValuePair<Item, int>(new Item("Item3"), 3),
            });

            _repository = new OpeningDelayRepository(settings, new LocalTimeProvider(), new MemoryJsonSaveRepository());
            _command = new OpeningDelayCommand(_board, _repository);
        }

        [UnityTest]
        public IEnumerator Execute()
        {
            Assert.Throws<CommandException>(() => _command.Execute(0, new MapCoordinate(1, 0)));

            yield return new WaitForSeconds(1.5f);
            Assert.DoesNotThrow(() => _command.Execute(0, new MapCoordinate(1, 0)));
        }

        [Test]
        public void Undo()
        {
            Assert.Throws<InvalidOperationException>(() => _command.Undo());

            Assert.Throws<CommandException>(() => _command.Execute(0, new MapCoordinate(1, 0)));
            Assert.DoesNotThrow(() => _command.Undo());
            Assert.Throws<InvalidOperationException>(() => _command.Undo());
        }
    }
}
