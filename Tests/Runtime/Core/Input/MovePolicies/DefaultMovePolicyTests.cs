using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.InputTests.MovePoliciesTests
{
    public class DefaultMovePolicyTests
    {
        [Test]
        public void ShouldSwapOpenedItems()
        {
            string firstItemId = "1";
            string secondItemId = "2";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(firstItemId)},
                {secondItemPosition, new Item(secondItemId)}
            });

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsTrue(board.Item(firstItemPosition).Id == firstItemId && board.Item(secondItemPosition).Id == secondItemId);
            Assert.IsTrue(movePolicy.CanMove(firstItemPosition, secondItemPosition));
            
            movePolicy.Move(firstItemPosition, secondItemPosition);

            Assert.IsTrue(board.Item(firstItemPosition).Id == secondItemId && board.Item(secondItemPosition).Id == firstItemId);
        }
        
        [Test]
        public void ShouldMoveItemIfToPositionFree()
        {
            string itemId = "1";
            var itemPosition = new MapCoordinate(1, 2);
            var toPosition = new MapCoordinate(2, 1);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {itemPosition, new Item(itemId)}
            });
            board.Open(toPosition);

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsTrue(board.Item(itemPosition).Id == itemId && board.HasItem(toPosition) == false);
            Assert.IsTrue(movePolicy.CanMove(itemPosition, toPosition));
            
            movePolicy.Move(itemPosition, toPosition);

            Assert.IsTrue(board.Item(toPosition).Id == itemId && board.HasItem(itemPosition) == false);
        }
        
        [Test]
        public void ShouldNotThrowIfFromEqualsTo()
        {
            string itemId = "1";
            var itemPosition = new MapCoordinate(1, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {itemPosition, new Item(itemId)}
            });

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsTrue(movePolicy.CanMove(itemPosition, itemPosition));
            Assert.DoesNotThrow(() => movePolicy.Move(itemPosition, itemPosition));
        }
        
        [Test]
        public void ShouldThrowExceptionIfItemLocked()
        {
            string firstItemId = "1";
            string secondItemId = "2";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(firstItemId)},
                {secondItemPosition, new Item(secondItemId)}
            }, openItems: false);

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, secondItemPosition));

            firstItemPosition = new MapCoordinate(3, 2);
            secondItemPosition = new MapCoordinate(4, 4);
            board.Add(new Item(firstItemId), firstItemPosition);
            board.Add(new Item(secondItemId), secondItemPosition);
            board.Open(firstItemPosition);

            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, secondItemPosition));
        }
        
        [Test]
        public void ShouldThrowExceptionIfItemContour()
        {
            string firstItemId = "1";
            string secondItemId = "2";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(firstItemId)},
                {secondItemPosition, new Item(secondItemId)}
            }, false);
            
            board.Open(firstItemPosition);
            Assert.IsFalse(board.Contour(firstItemPosition));
            Assert.IsTrue(board.Contour(secondItemPosition));

            IMovePolicy movePolicy = new DefaultMovePolicy(board);

            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, secondItemPosition));
            
            firstItemPosition = new MapCoordinate(3, 2);
            secondItemPosition = new MapCoordinate(3, 3);
            board.Add(new Item(firstItemId), firstItemPosition);
            board.Add(new Item(secondItemId), secondItemPosition);
            board.Open(firstItemPosition);
            
            Assert.IsFalse(movePolicy.CanMove(secondItemPosition, firstItemPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(secondItemPosition, firstItemPosition));
        }
        
        [Test]
        public void ShouldThrowExceptionIfToPositionOutOfBounds()
        {
            string itemId = "1";
            var itemPosition = new MapCoordinate(1, 2);
            var toPosition = new MapCoordinate(15, 15);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {itemPosition, new Item(itemId)}
            });

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsFalse(movePolicy.CanMove(itemPosition, toPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(itemPosition, toPosition));
        }
        
        [Test]
        public void ShouldThrowExceptionIfFirstItemEqualsSecondItem()
        {
            string itemsId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            var secondItemPosition = new MapCoordinate(2, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(itemsId)},
                {secondItemPosition, new Item(itemsId)}
            });

            IMovePolicy movePolicy = new DefaultMovePolicy(board);
            
            Assert.IsTrue(board.Item(firstItemPosition).Id == itemsId && board.Item(secondItemPosition).Id == itemsId);
            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, secondItemPosition));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, secondItemPosition));
        }

        [Test]
        public void ShouldThrowIfSecondPositionLockedOrContour()
        {
            string firstItemId = "1";
            var firstItemPosition = new MapCoordinate(1, 2);
            
            IBoard board = CreateBoard(new Dictionary<MapCoordinate, Item>
            {
                {firstItemPosition, new Item(firstItemId)},
            }, false);
            
            board.Open(firstItemPosition);
            Assert.IsFalse(board.Contour(firstItemPosition));

            IMovePolicy movePolicy = new DefaultMovePolicy(board);

            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, new MapCoordinate(5, 5)));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, new MapCoordinate(5, 5)));
            
            firstItemPosition = new MapCoordinate(3, 2);
            board.Add(new Item(firstItemId), firstItemPosition);
            board.Open(new MapCoordinate(4, 4));
            
            Assert.IsTrue(board.Contour(new MapCoordinate(4, 5)));
            
            Assert.IsFalse(movePolicy.CanMove(firstItemPosition, new MapCoordinate(4, 5)));
            Assert.Throws<InvalidOperationException>(() => movePolicy.Move(firstItemPosition, new MapCoordinate(4, 5)));
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
