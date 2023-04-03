using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.OpenedItemListTests
{
    public class OpenedItemListTests
    {
        private Dictionary<MapCoordinate, Item> _itemsWithPosition;
        
        [SetUp]
        public void Initialize()
        {
            _itemsWithPosition = new Dictionary<MapCoordinate, Item>()
            {
                {new MapCoordinate(0, 0), new Item("1")}, {new MapCoordinate(1, 0), new Item("2")}, {new MapCoordinate(2, 0), new Item("3")},
                {new MapCoordinate(0, 1), new Item("4")}, {new MapCoordinate(1, 1), new Item("5")}, {new MapCoordinate(2, 1), new Item("6")},
                {new MapCoordinate(0, 2), new Item("7")}, {new MapCoordinate(1, 2), new Item("8")}, {new MapCoordinate(2, 2), new Item("9")},
            };
        }
        
        [Test]
        public void ItemsAddedToListIfOpenedOrContour()
        {
            var board = new Board(new RectShape(3, 3), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());

            foreach (var pair in _itemsWithPosition)
                board.Add(pair.Value, pair.Key);

            var openedItemList = new OpenedItemList(board);
            
            Assert.IsFalse(openedItemList.OpenedItems.Any());

            var openPosition = new MapCoordinate(1, 1);
            board.Open(openPosition);
            
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[openPosition]));
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(1, 2)]));
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(2, 1)]));
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(1, 0)]));
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(0, 1)]));

            Assert.IsFalse(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(0, 0)]));
            Assert.IsFalse(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(2, 0)]));
            Assert.IsFalse(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(0, 2)]));
            Assert.IsFalse(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(2, 2)]));

            board.Open(new MapCoordinate(0, 2));
            
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[new MapCoordinate(0, 2)]));
            
            board.Remove(openPosition);
            
            Assert.IsTrue(openedItemList.OpenedItems.Contains(_itemsWithPosition[openPosition]));

            Item addedItem = new Item("1", 1);
            board.Add(addedItem, openPosition);
            
            Assert.IsTrue(openedItemList.OpenedItems.Contains(addedItem));
            
            openedItemList.Dispose();
        }
        
        [Test]
        public void ShouldHaveAllOpenedItemsFromBoardAfterCreate()
        {
            var board = new Board(new RectShape(3, 3), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());

            foreach (var pair in _itemsWithPosition)
                board.Add(pair.Value, pair.Key);

            board.Open(new MapCoordinate(1, 1));
            
            var openedItemList = new OpenedItemList(board);
            
            Assert.AreEqual(5, openedItemList.OpenedItems.Count());
        }
    }
}
