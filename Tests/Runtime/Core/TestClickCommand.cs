using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core.Tests
{
    internal class TestClickCommand : IClickCommand
    {
        public TestClickCommand(string info = "")
        {
            Info = info;
        }

        public string Info { get; private set; }
        public bool ExecuteCalled { get; private set; }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {
            ExecuteCalled = true;
        }

        void IClickCommand.Undo()
        {
            if (ExecuteCalled == false)
                throw new InvalidOperationException();

            ExecuteCalled = false;
        }

        public void Clear()
        {
            ExecuteCalled = false;
        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>();
        }
    }

    internal class TestQueueCommand : IClickCommand
    {
        private IEnumerable<IClickCommand> _commands;

        public TestQueueCommand(IEnumerable<IClickCommand> commands)
        {
            _commands = commands;
        }

        void IClickCommand.Execute(int itemLevel, MapCoordinate clickPosition)
        {

        }

        IEnumerable<T> IClickCommand.Filter<T>()
        {
            return this.CreateFilter<T>(_commands);
        }

        void IClickCommand.Undo()
        {

        }
    }

    internal class TestCommand1 : TestClickCommand
    {
        public TestCommand1(string info)
            : base(info) { }
    }
    internal class TestCommand2 : TestClickCommand
    {
        public TestCommand2(string info)
            : base(info)
        { }
    }
    internal class TestCommand3 : TestClickCommand
    {
        public TestCommand3(string info)
            : base(info)
        { }
    }
    internal class TestCommand4 : TestClickCommand
    {
        public TestCommand4(string info)
            : base(info)
        { }
    }
}