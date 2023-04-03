using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public static class FilterFactory
    {
        public static IEnumerable<T> CreateFilter<T>(this IClickCommand clickCommand) where T : IClickCommand
        {
            if (clickCommand is T)
                return new[] { (T)Convert.ChangeType(clickCommand, typeof(T)) };

            return Array.Empty<T>();
        }

        public static IEnumerable<T> CreateFilter<T>(this IClickCommand clickCommand, IEnumerable<IClickCommand> otherCommands) where T : IClickCommand
        {
            var result = new List<T>(clickCommand.CreateFilter<T>());

            foreach (var command in otherCommands)
                result.AddRange(command.Filter<T>());

            return result;
        }
    }
}
