using NUnit.Framework;
using System;

namespace Agava.Merge2.Core.Tests.BoardTests.ShapeTests
{
    public class RectShapeTests
    {
        [Test]
        public void ShouldValidWidthHeight()
        {
            var shape = new RectShape(5, 5);
            Assert.AreEqual(shape.Width, 5);
            Assert.AreEqual(shape.Height, 5);

            shape = new RectShape(10, 999);
            Assert.AreEqual(shape.Width, 10);
            Assert.AreEqual(shape.Height, 999);

            shape = new RectShape(987654, 1234);
            Assert.AreEqual(shape.Width, 987654);
            Assert.AreEqual(shape.Height, 1234);

            shape = new RectShape(1, 1);
            Assert.AreEqual(shape.Width, 1);
            Assert.AreEqual(shape.Height, 1);
        }

        [Test]
        public void ShouldConstructorException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(5, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(-2, 999));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(999, -10));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(-10, -10));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(-195, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(int.MinValue, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(10, int.MinValue));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(unchecked(int.MaxValue + 1), 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RectShape(1, unchecked(int.MaxValue + 1)));
        }

        [Test]
        public void ShouldContains()
        {
            var shape = new RectShape(5, 2);
            Assert.IsTrue(shape.Contains(new MapCoordinate()));

            shape = new RectShape(10, 2);
            Assert.IsTrue(shape.Contains(new MapCoordinate(5, 1)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(9, 1)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(1, 0)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(9, 0)));

            shape = new RectShape(12, 20);
            Assert.IsTrue(shape.Contains(new MapCoordinate(0, 11)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(11, 19)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(5, 5)));
            Assert.IsTrue(shape.Contains(new MapCoordinate(0, 0)));
        }

        [Test]
        public void ShouldNotContains()
        {
            var shape = new RectShape(5, 2);
            Assert.IsFalse(shape.Contains(new MapCoordinate(5, 0)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(0, 2)));

            shape = new RectShape(10, 2);
            Assert.IsFalse(shape.Contains(new MapCoordinate(0, 2)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(10, 0)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(10, 2)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(999, 999)));

            shape = new RectShape(12, 20);
            Assert.IsFalse(shape.Contains(new MapCoordinate(20, 12)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(19, 11)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(0, 20)));
            Assert.IsFalse(shape.Contains(new MapCoordinate(19, 19)));
        }

        [Test]
        public void Superset()
        {
            var shape = new RectShape(1, 1);
            Assert.IsTrue(shape.IsSupersetOf(new[] { new MapCoordinate(0, 0) }));
            Assert.IsFalse(shape.IsSupersetOf(new[] { new MapCoordinate(0, 0), new MapCoordinate(1, 1) }));

            shape = new RectShape(10, 5);

            Assert.IsTrue(shape.IsSupersetOf(new[] { new MapCoordinate(0, 0) }));
            Assert.IsTrue(shape.IsSupersetOf(new[] { new MapCoordinate(5, 0), new MapCoordinate(0, 3), new MapCoordinate(5, 4) }));
            Assert.IsFalse(shape.IsSupersetOf(new[] { new MapCoordinate(15, 0), new MapCoordinate(0, 3), new MapCoordinate(5, 4) }));
            Assert.IsFalse(shape.IsSupersetOf(new[] { new MapCoordinate(0, 5)}));
        }
    }
}
