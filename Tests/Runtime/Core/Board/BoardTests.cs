using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core.Tests.BoardTests
{
    public class BoardTests
    {
        [Test]
        public void ShouldAddAndRemoveIfCan()
        {
            IBoard board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());

            Assert.DoesNotThrow(() => board.Add(new Item("0"), new MapCoordinate(2, 2)));
            Assert.DoesNotThrow(() => board.Add(new Item("0"), new MapCoordinate(9, 9)));
            Assert.DoesNotThrow(() => board.Add(new Item("0"), new MapCoordinate(0, 0)));
            Assert.DoesNotThrow(() => board.Add(new Item("0"), new MapCoordinate(1, 8)));

            Assert.Throws<InvalidOperationException>(() => board.Add(new Item("0"), new MapCoordinate(2, 2)));
            Assert.Throws<InvalidOperationException>(() => board.Add(new Item("0"), new MapCoordinate(2, 16)));

            Assert.DoesNotThrow(() => board.Remove(new MapCoordinate(0, 0)));
            Assert.DoesNotThrow(() => board.Remove(new MapCoordinate(9, 9)));

            Assert.Throws<InvalidOperationException>(() => board.Remove(new MapCoordinate(0, 0)));
            Assert.Throws<InvalidOperationException>(() => board.Remove(new MapCoordinate(14, 1)));
        }

        [Test]
        public void ShouldReturnItemIfExist()
        {
            IBoard board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());
            board.Add(new Item("one"), new MapCoordinate(0, 1));
            board.Add(new Item("two"), new MapCoordinate(0, 2));
            board.Add(new Item("tree"), new MapCoordinate(0, 3));

            Assert.True(board.HasItem(new MapCoordinate(0, 1)));
            Assert.True(board.HasItem(new MapCoordinate(0, 2)));
            Assert.True(board.HasItem(new MapCoordinate(0, 3)));

            Assert.False(board.HasItem(new MapCoordinate(0, 4)));
            Assert.False(board.HasItem(new MapCoordinate(2, 0)));

            Assert.Throws<InvalidOperationException>(() => board.Item(new MapCoordinate(1, 0)));
            Assert.Throws<InvalidOperationException>(() => board.Item(new MapCoordinate(0, 11)));

            Assert.That(board.Item(new MapCoordinate(0, 2)).Equals(new Item("two")));
            Assert.That(board.Item(new MapCoordinate(0, 2)).Equals(new Item("two")));
            Assert.That(board.Item(new MapCoordinate(0, 3)).Equals(new Item("tree")));
        }

        [Test]
        public void ShouldOpenIfCan()
        {
            IBoard board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());
            Assert.AreEqual(0, board.OpenedCollection.Count);
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    Assert.False(board.Opened(new MapCoordinate(x, y)));

            board = new Board(new RectShape(10, 10), new RectContourAlgorithm(),
                new[] { new MapCoordinate(), new MapCoordinate(3, 4), new MapCoordinate(4, 3), new MapCoordinate(1, 1) });

            Assert.AreEqual(4, board.OpenedCollection.Count);
            Assert.True(board.Opened(new MapCoordinate(0, 0)));
            Assert.True(board.Opened(new MapCoordinate(3, 4)));
            Assert.True(board.Opened(new MapCoordinate(4, 3)));
            Assert.True(board.Opened(new MapCoordinate(1, 1)));

            Assert.False(board.Opened(new MapCoordinate(5, 4)));
            board.Open(new MapCoordinate(5, 4));
            Assert.AreEqual(5, board.OpenedCollection.Count);
            Assert.True(board.Opened(new MapCoordinate(5, 4)));
        }

        [Test]
        public void OpenedCollectionAndContourShouldNotOverlaps()
        {
            var board = new Board(new RectShape(10, 10), new RectContourAlgorithm(),
                new[] { new MapCoordinate(), new MapCoordinate(3, 4), new MapCoordinate(4, 3), new MapCoordinate(1, 1) });

            var openedHash = new HashSet<MapCoordinate>(board.OpenedCollection);
            Assert.That(openedHash.Overlaps(board.ContourCollection) == false);

            board.Open(new MapCoordinate(0, 2));
            board.Open(new MapCoordinate(0, 4));

            openedHash = new HashSet<MapCoordinate>(board.OpenedCollection);
            Assert.That(openedHash.Overlaps(board.ContourCollection) == false);
        }

        [Test]
        public void ShouldThrowWhenCreateWithWrongArgument()
        {
            Assert.Throws<ArgumentException>(() => new Board(new RectShape(10, 10), new RectContourAlgorithm(), new[] { new MapCoordinate(10, 10) }));
            Assert.Throws<ArgumentException>(() => new Board(new RectShape(1, 3), new RectContourAlgorithm(), new[] { new MapCoordinate(0, 1), new MapCoordinate(1, 0)}));
        }
    }
}
