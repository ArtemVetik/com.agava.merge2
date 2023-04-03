using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.InputTests.MergePoliciesTests
{
    public class DefaultMergePolicyTests
    {
        [Test]
        public void Merge_ItemShouldBeInSecondCoordinateAndNotBeInFirstCoordinate()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
                {secondItemPosition, new Item(itemId)}
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });
            
            Assert.IsTrue(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            
            mergePolicy.Merge(firstItemPosition, secondItemPosition);
            
            Assert.IsTrue(board.HasItem(secondItemPosition) && board.HasItem(firstItemPosition) == false);
        }
        
        [Test]
        public void Merge_CanMergeIfSecondItemInContour()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
                {secondItemPosition, new Item(itemId)}
            }, false);
            
            board.Open(firstItemPosition);
            Assert.IsFalse(board.Contour(firstItemPosition));
            Assert.IsTrue(board.Contour(secondItemPosition));

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });

            Assert.IsTrue(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.DoesNotThrow(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
            
            firstItemPosition = new MapCoordinate(3, 2);
            secondItemPosition = new MapCoordinate(3, 3);
            board.Add(new Item(itemId), firstItemPosition);
            board.Add(new Item(itemId), secondItemPosition);
            board.Open(firstItemPosition);
            
            Assert.IsFalse(mergePolicy.CanMerge(secondItemPosition, firstItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(secondItemPosition, firstItemPosition));
        }

        [Test]
        public void Merge_ShouldThrowExceptionWhenItemIsLocked()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
                {secondItemPosition, new Item(itemId)}
            }, openItems: false);

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));

            firstItemPosition = new MapCoordinate(3, 2);
            secondItemPosition = new MapCoordinate(4, 4);
            board.Add(new Item(itemId), firstItemPosition);
            board.Add(new Item(itemId), secondItemPosition);
            board.Open(firstItemPosition);

            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
        }

        [Test]
        public void Merge_ShouldThrowExceptionWhenItemIsMaxLevel()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId, 0)},
                {secondItemPosition, new Item(itemId, 0)}
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 0} });
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
        }
        
        [Test]
        public void Merge_ShouldThrowExceptionWhenItemIdIsNotInDictionary()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId, 0)},
                {secondItemPosition, new Item(itemId, 0)}
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board);
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
        }
        
        [Test]
        public void Merge_ShouldThrowExceptionWhenNoItemInCoordinate()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
        }
        
        [Test]
        public void Merge_ShouldThrowExceptionWhenMergeDifferentItems()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
                {secondItemPosition, new Item("2")},
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10}, {"2", 10}});
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, secondItemPosition));
        }
        
        [Test]
        public void Merge_ShouldThrowExceptionWhenMergeOneItems()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
            });

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });
            
            Assert.IsFalse(mergePolicy.CanMerge(firstItemPosition, firstItemPosition));
            Assert.Throws<InvalidOperationException>(() => mergePolicy.Merge(firstItemPosition, firstItemPosition));
        }
        
        [Test]
        public void ShouldOpenCellAfterMerge()
        {
            string itemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(1, 3);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemId)},
                {secondItemPosition, new Item(itemId)}
            }, openItems: false);
            board.Open(firstItemPosition);
            
            Assert.IsTrue(board.Contour(secondItemPosition));

            IMergePolicy mergePolicy = new DefaultMergePolicy(board, new Dictionary<string, int> { {itemId, 10} });
            
            Assert.IsTrue(mergePolicy.CanMerge(firstItemPosition, secondItemPosition));
            
            mergePolicy.Merge(firstItemPosition, secondItemPosition);
            
            Assert.IsTrue(board.HasItem(secondItemPosition) && board.HasItem(firstItemPosition) == false);
            Assert.IsTrue(board.Opened(secondItemPosition));
        }

        private IBoard CreateBoard(Dictionary<MapCoordinate, Item> coordinatesWithItem, bool openItems = true)
        {
            IBoard board = new Board(new RectShape(10, 10), new RectContourAlgorithm(), Array.Empty<MapCoordinate>());

            foreach (var pair in coordinatesWithItem)
            {
                board.Add(pair.Value, pair.Key);
                
                if (openItems)
                    board.Open(pair.Key);
            }

            return board;
        }
    }
}
