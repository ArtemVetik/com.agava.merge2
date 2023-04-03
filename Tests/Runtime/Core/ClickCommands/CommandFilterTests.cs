using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core.Tests.ClickCommandsTests
{
    public class CommandFilterTests
    {
        private CommandFilter _filter;

        [SetUp]
        public void Initialize()
        {
            _filter = new CommandFilter(new[]
            {
                new KeyValuePair<string, IClickCommand>("Item1", new TestCommand1("Item1_TestCommand1")),
                new KeyValuePair<string, IClickCommand>("Item2", new TestCommand2("Item2_TestCommand2")),
                new KeyValuePair<string, IClickCommand>("Item3", new TestQueueCommand(new IClickCommand[]
                {
                    new TestCommand2("Item3_TestCommand2"),
                    new TestCommand1("Item3_TestCommand1"),
                })),
                new KeyValuePair<string, IClickCommand>("Item4", new TestQueueCommand(new IClickCommand[]
                {
                    new TestCommand3("Item4_TestCommand3"),
                    new TestQueueCommand(new []
                    {
                        new TestCommand2("Item4_TestCommand2.1"),
                        new TestCommand2("Item4_TestCommand2.2"),
                    })
                })),
            });
        }

        [Test]
        public void ShouldReturnEmptyArray()
        {
            Assert.AreEqual(0, _filter.Filter<TestCommand4>().Count());
            Assert.AreEqual(0, _filter.Filter<ProduceCommand>().Count());
        }

        [Test]
        public void ShouldReturnNotEmptyArray()
        {
            Assert.AreEqual(2, _filter.Filter<TestCommand1>().Count());
            Assert.AreEqual(3, _filter.Filter<TestCommand2>().Count());
            Assert.AreEqual(1, _filter.Filter<TestCommand3>().Count());
            Assert.AreEqual(2, _filter.Filter<TestQueueCommand>().Count());
        }

        [Test]
        public void ShouldReturnCorrectIDAndCommands()
        {
            var filter1 = _filter.Filter<TestCommand1>().ToArray();
            Assert.AreEqual(1, filter1[0].Value.Count());
            Assert.AreEqual("Item1", filter1[0].Key);
            Assert.AreEqual("Item1_TestCommand1", filter1[0].Value.ToArray()[0].Info);
            Assert.AreEqual(1, filter1[1].Value.Count());
            Assert.AreEqual("Item3", filter1[1].Key);
            Assert.AreEqual("Item3_TestCommand1", filter1[1].Value.ToArray()[0].Info);

            var filter2 = _filter.Filter<TestCommand2>().ToArray();
            Assert.AreEqual(2, filter2[2].Value.Count());
            Assert.AreEqual("Item4", filter2[2].Key);
            Assert.AreEqual("Item4_TestCommand2.1", filter2[2].Value.ToArray()[0].Info);
            Assert.AreEqual("Item4_TestCommand2.2", filter2[2].Value.ToArray()[1].Info);

            var filter3 = _filter.Filter<TestQueueCommand>().ToArray();
            Assert.AreEqual(2, filter3[1].Value.Count());
            Assert.AreEqual("Item3", filter3[0].Key);
            Assert.AreEqual("Item4", filter3[1].Key);
        }
    }
}
