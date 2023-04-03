using NUnit.Framework;
using System;

namespace Agava.Merge2.Core.Tests.InputTests
{
    public class ClickPolicyTests
    {
        private IBoard _board;
        private ClickPolicy _clickPolicy;

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

            _clickPolicy = new ClickPolicy(_board, ("0", new TestClickCommand()));
        }

        [Test]
        public void ShouldThrowWhenTryClickToEmptyCell()
        {
            Assert.Throws<InvalidOperationException>(() => _clickPolicy.Click(new MapCoordinate()));
        }

        [Test]
        public void ShouldThrowItemHasNoClickCommand()
        {
            _board.Add(new Item("1"), new MapCoordinate(5, 5));
            Assert.Throws<InvalidOperationException>(() => _clickPolicy.Click(new MapCoordinate(5, 5)));
        }

        [Test]
        public void ShouldExecuteCommand()
        {
            _board.Add(new Item("0"), new MapCoordinate(1, 2));
            Assert.DoesNotThrow(() => _clickPolicy.Click(new MapCoordinate(1, 2)));
        }
    }
}
