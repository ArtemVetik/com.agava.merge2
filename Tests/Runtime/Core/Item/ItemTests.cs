using NUnit.Framework;

namespace Agava.Merge2.Core.Tests.ItemsTests
{
    public class ItemTests
    {
        [Test]
        public void ShouldReturnSameNextLevelItem()
        {
            var item = new Item("1", 1);
            var nextItem = item.Next();
            
            Assert.IsTrue(item.Id == nextItem.Id && item.Level == nextItem.Level - 1);
        }

        [Test]
        public void ShouldReturnTrueIfItemEquals()
        {
            var item1 = new Item("11", 1);
            var item2 = new Item("11", 1);
            var item3 = new Item("11", 2);
            
            Assert.IsTrue(item1.Equals(item2));
            Assert.IsTrue(item1 == item2);

            Assert.IsTrue(item3.Equals(item2.Next()));
            Assert.IsTrue(item3 ==item2.Next());

            Assert.IsTrue(item1 != item3);
            Assert.IsTrue(item1 != item1.Next());
        }
    }
}
