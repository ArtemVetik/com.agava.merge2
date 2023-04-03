using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.BoardTests.ShapeTests
{
    public class ArbitaryShapeTests
    {
        private ArbitaryShape _shape;

        [SetUp]
        public void Initialize()
        {
            _shape = new ArbitaryShape(new[]
            {
                new MapCoordinate(2, 0), new MapCoordinate(1, 1), new MapCoordinate(2, 1), new MapCoordinate(3, 1),
                new MapCoordinate(0, 2), new MapCoordinate(1, 2), new MapCoordinate(2, 2), new MapCoordinate(3, 2), new MapCoordinate(4, 2),
                new MapCoordinate(1, 3), new MapCoordinate(2, 3), new MapCoordinate(3, 3), new MapCoordinate(2, 4),
            });
        }

        [Test]
        public void ShouldValidWidthHeight()
        {
            Assert.AreEqual(5, _shape.Width);
            Assert.AreEqual(5, _shape.Height);
        }

        [Test]
        public void ShouldContains()
        {
            Assert.True(_shape.Contains(new MapCoordinate(1, 2)));
            Assert.True(_shape.Contains(new MapCoordinate(2, 4)));
            Assert.True(_shape.Contains(new MapCoordinate(3, 1)));
        }

        [Test]
        public void ShouldNotContains()
        {
            Assert.False(_shape.Contains(new MapCoordinate(0, 0)));
            Assert.False(_shape.Contains(new MapCoordinate(3, 0)));
            Assert.False(_shape.Contains(new MapCoordinate(0, 3)));
            Assert.False(_shape.Contains(new MapCoordinate(4, 3)));
        }

        [Test]
        public void Superset()
        {
            Assert.True(_shape.IsSupersetOf(new[]
            {
                new MapCoordinate(1, 1), new MapCoordinate(1, 3), new MapCoordinate(2, 2), new MapCoordinate(4, 2),
            }));
        }
    }
}
