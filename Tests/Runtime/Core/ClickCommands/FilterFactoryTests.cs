using NUnit.Framework;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class FilterFactoryTests
    {
        [Test]
        public void ShouldCreateSingleArray()
        {
            var command1 = new TestCommand1("Command1");

            Assert.AreEqual(0, FilterFactory.CreateFilter<TestCommand2>(command1).Count());

            var result1 = FilterFactory.CreateFilter<TestCommand1>(command1);
            Assert.AreEqual(1, result1.Count());
            Assert.AreEqual("Command1", result1.ToArray()[0].Info);
        }

        [Test]
        public void ShouldCreateMultiplyArray()
        {
            var command1 = new TestCommand1("1");
            var command11 = new TestCommand1("11");
            var command2 = new TestCommand2("2");
            var command31 = new TestCommand3("31");
            var command32 = new TestCommand3("32");
            var command33 = new TestCommand3("33");
            var command4 = new TestCommand4("4");

            var res1 = FilterFactory.CreateFilter<TestCommand1>(command1, new IClickCommand[] { command2, command11 });
            Assert.AreEqual(2, res1.Count());

            var res2 = FilterFactory.CreateFilter<TestCommand3>(command33, new IClickCommand[] { command32, command31 });
            Assert.AreEqual(3, res2.Count());

            var res3 = FilterFactory.CreateFilter<TestCommand4>(command2, new IClickCommand[] { command1 });
            Assert.AreEqual(0, res3.Count());
            
            var res4 = FilterFactory.CreateFilter<TestCommand4>(command2, new IClickCommand[] { command4 });
            Assert.AreEqual(1, res4.Count());
        }
    }
}
