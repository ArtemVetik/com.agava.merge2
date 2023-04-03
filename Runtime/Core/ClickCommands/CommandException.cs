using System;

namespace Agava.Merge2.Core
{
    public class CommandException : Exception
    {
        public CommandException()
        {

        }

        public CommandException(string message) : base(message)
        {
        }
    }
}
