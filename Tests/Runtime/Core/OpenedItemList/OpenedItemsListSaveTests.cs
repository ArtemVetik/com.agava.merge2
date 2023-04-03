using Agava.Merge2.Tests;
using NUnit.Framework;
using System;
using System.Linq;

namespace Agava.Merge2.Core.Tests.OpenedItemListTests
{
    public class OpenedItemsListSaveTests
    {
        [Test]
        public void ShouldSaveAndLoadItems()
        {
            var board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());
            var openedItemsSave = new OpenedItemListSave(new MemoryJsonSaveRepository());
            var openedItems = openedItemsSave.Load(board);

            Assert.AreEqual(0, openedItems.OpenedItems.Count());

            openedItems = new OpenedItemList(board, new[]
            {
                new Item("Item1"), new Item("Item2"), new Item("Item3"), new Item("Item4"),
            });

            openedItemsSave.Save(openedItems);
            openedItems = openedItemsSave.Load(board);
            Assert.AreEqual(4, openedItems.OpenedItems.Count());
        }
    }
}
