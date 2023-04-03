using Agava.Merge2.Core;
using NUnit.Framework;
using System;

namespace Agava.Merge2.Tasks.Tests
{
    public class TaskTests
    {
        [Test]
        public void ShouldThrowIfEmptyTotalItemsInConstructor()
        {
            Assert.Throws<ArgumentException>(() => new Task(Array.Empty<Item>()));
        }
    }
}