using NUnit.Framework;
using System;

namespace Agava.Merge2.Core.Tests.BoardTests
{
    public class MapCoordinateTests
    {
        [Test]
        public void ShouldValidConstructor()
        {
            Assert.DoesNotThrow(() => new MapCoordinate());
            Assert.DoesNotThrow(() => new MapCoordinate(0, 0));
            Assert.DoesNotThrow(() => new MapCoordinate(1, 0));
            Assert.DoesNotThrow(() => new MapCoordinate(0, 1));
            Assert.DoesNotThrow(() => new MapCoordinate(999, 1234));
        }

        [Test]
        public void ShouldInvalidConstructor()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MapCoordinate(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MapCoordinate(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MapCoordinate(-123, -321));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MapCoordinate(unchecked(int.MaxValue + 1), 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MapCoordinate(0, unchecked(int.MaxValue + 1)));
        }

        [Test]
        public void EqualsOrNot()
        {
            var firstCoordinate = new MapCoordinate();
            var secondCoordinate = new MapCoordinate(0, 0);
            Assert.AreEqual(firstCoordinate, secondCoordinate);
            Assert.IsTrue(firstCoordinate.Equals(secondCoordinate));

            secondCoordinate = new MapCoordinate(1, 0);
            Assert.AreNotEqual(firstCoordinate, secondCoordinate);
            Assert.IsFalse(firstCoordinate.Equals(secondCoordinate));

            firstCoordinate = new MapCoordinate(1, 0);
            Assert.AreEqual(firstCoordinate, secondCoordinate);
            Assert.IsTrue(firstCoordinate.Equals(secondCoordinate));

            firstCoordinate = new MapCoordinate(int.MaxValue, int.MaxValue);
            secondCoordinate = new MapCoordinate(int.MaxValue, int.MaxValue);
            Assert.AreEqual(firstCoordinate, secondCoordinate);
            Assert.IsTrue(firstCoordinate.Equals(secondCoordinate));
        }

        [Test]
        public void ShouldCompleteAndCorrectIfValidOperations()
        {
            var firstCoordinate = new MapCoordinate(10, 5);
            var secondCoordinate = new MapCoordinate(5, 10);
            var result = new MapCoordinate();
            Assert.DoesNotThrow(() => result = firstCoordinate + secondCoordinate);
            result = firstCoordinate + secondCoordinate;
            Assert.AreEqual(result, new MapCoordinate(15, 15));

            firstCoordinate = new MapCoordinate(int.MaxValue, int.MaxValue);
            secondCoordinate = new MapCoordinate(1, 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => result = firstCoordinate + secondCoordinate);

            firstCoordinate = new MapCoordinate(int.MaxValue - 1, int.MaxValue);
            secondCoordinate = new MapCoordinate(1, 0);
            Assert.DoesNotThrow(() => result = firstCoordinate + secondCoordinate);
            result = firstCoordinate + secondCoordinate;
            Assert.AreEqual(result, new MapCoordinate(int.MaxValue, int.MaxValue));
        }
    }
}
