using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core.Tests.BoardTests.ContourTests
{
    public class RectContourAlgorithmTests
    {
        private List<TestData> _testDataList;

        [SetUp]
        public void Initialize()
        {
            _testDataList = new List<TestData>()
            {
                new TestData()
                {
                    Input = new [] { new MapCoordinate()},
                    ExpectedResult = new [] { new MapCoordinate(0, 1), new MapCoordinate(1, 0)},
                },
                new TestData()
                {
                    Input = new [] { new MapCoordinate(5, 5), new MapCoordinate(5, 6), new MapCoordinate(5, 7)},
                    ExpectedResult = new [] { new MapCoordinate(5, 4), new MapCoordinate(5, 8),
                                                new MapCoordinate(4, 5), new MapCoordinate(6, 5),
                                                new MapCoordinate(4, 6), new MapCoordinate(6, 6),
                                                new MapCoordinate(4, 7), new MapCoordinate(6, 7)},
                },
                new TestData()
                {
                    Input = new [] { new MapCoordinate(1, 1), new MapCoordinate(2, 1), new MapCoordinate(3, 1),
                                        new MapCoordinate(2, 2), new MapCoordinate(3, 2), new MapCoordinate(4, 2),
                                        new MapCoordinate(3, 3), new MapCoordinate(4, 3)},
                    ExpectedResult = new [] { new MapCoordinate(0, 1), new MapCoordinate(1, 0), new MapCoordinate(2, 0),
                                                new MapCoordinate(3, 0), new MapCoordinate(4, 1), new MapCoordinate(5, 2),
                                                new MapCoordinate(5, 3), new MapCoordinate(4, 4), new MapCoordinate(3, 4),
                                                new MapCoordinate(2, 3), new MapCoordinate(1, 2)},
                },
            };
        }

        [Test]
        public void ShouldCorrectlyCompute()
        {
            var contour = new RectContourAlgorithm();

            foreach (var data in _testDataList)
            {
                var result = contour.Compute(data.Input);
                Assert.AreEqual(data.ExpectedResult.Count, result.Count());

                foreach (var expected in data.ExpectedResult)
                    Assert.That(result.Contains(expected), () => $"Input: {string.Join(", ", data.Input)}\nExpected: {expected}");
            }
        }

        private struct TestData
        {
            public ICollection<MapCoordinate> Input;
            public ICollection<MapCoordinate> ExpectedResult;
        }
    }
}
