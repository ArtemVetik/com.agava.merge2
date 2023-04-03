using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ItemsTests
{
    public class ItemProduceInfoTests
    {
        private ItemProduceInfo _itemInfo;

        [SetUp]
        public void Initialize()
        {
            var board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());

            var item1ProducedItems = new ProduceItems[]
            {
                new ProduceItems(new Item("Item1")),
                new ProduceItems(new Item("Item1"), new Item("Item2")),
            };
            var item2ProducedItems = new ProduceItems[]
            {
                new ProduceItems(new Item("Item3"), new Item("Item4")),
                new ProduceItems(new Item("Item3"), new Item("Item4"),new Item("Item5")),
            };
            var item3ProducedItems1 = new ProduceItems[]
            {
                new ProduceItems(new Item("Item1"), new Item("Item3")),
                new ProduceItems(new Item("Item2"), new Item("Item4")),
            };
            var item3ProducedItems2 = new ProduceItems[]
            {
                new ProduceItems(new Item("Item8"), new Item("Item9")),
                new ProduceItems(new Item("Item10"), new Item("Item11")),
            };
            var item3ProducedItems3 = new ProduceItems[]
            {
                new ProduceItems(new Item("Item5", 1), new Item("Item11")),
                new ProduceItems(new Item("Item12"), new Item("Item13")),
            };

            var filter = new CommandFilter(new[]
            {
                new KeyValuePair<string, IClickCommand>("Item3", new ProduceCommand(board, item1ProducedItems)),
                new KeyValuePair<string, IClickCommand>("Item5", new CommandQueue(new IClickCommand[]
                {
                    new ProduceCommand(board, item2ProducedItems),
                })),
                new KeyValuePair<string, IClickCommand>("Item7", new CommandQueue(new IClickCommand[]
                {
                    new ProduceCommand(board, item3ProducedItems1),
                    new TestQueueCommand(new []
                    {
                        new ProduceCommand(board, item3ProducedItems2),
                        new ProduceCommand(board, item3ProducedItems3),
                    })
                })),
            });

            _itemInfo = new ItemProduceInfo(filter);
        }

        [Test]
        public void ShouldReturnEmptyArrays()
        {
            var result = _itemInfo.Compute(new Item("abracadabra"));
            Assert.AreEqual(0, result.ProducedItems.Count);
            Assert.AreEqual(0, result.ProduceBy.Count);
        }

        [Test]
        public void Test()
        {
            var result = _itemInfo.Compute(new Item("Item1"));

            Assert.AreEqual(0, result.ProducedItems.Count);
            Assert.AreEqual(3, result.ProduceBy.Count);
            Assert.AreEqual(new Item("Item3", 0), result.ProduceBy[0]);
            Assert.AreEqual(new Item("Item3", 1), result.ProduceBy[1]);
            Assert.AreEqual(new Item("Item7", 0), result.ProduceBy[2]);

            result = _itemInfo.Compute(new Item("Item5", 1));
            Assert.AreEqual(3, result.ProducedItems.Count);
            Assert.AreEqual(2, result.ProduceBy.Count);
            Assert.AreEqual(new Item("Item3"), result.ProducedItems[0]);
            Assert.AreEqual(new Item("Item4"), result.ProducedItems[1]);
            Assert.AreEqual(new Item("Item5"), result.ProducedItems[2]);

            Assert.True(result.ProduceBy.Any(item => item.Equals(new Item("Item7"))));
            Assert.True(result.ProduceBy.Any(item => item.Equals(new Item("Item5", 1))));
        }
    }
}
