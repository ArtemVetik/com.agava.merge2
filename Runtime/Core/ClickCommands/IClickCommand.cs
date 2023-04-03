using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Provides an interface for creating click commands
    /// that are executed when clicking on a game item on the board.
    /// </summary>
    public interface IClickCommand
    {
        /// <summary>
        /// Command execution method.
        /// </summary>
        /// <param name="itemLevel">The level of the item on which the click occurs.</param>
        /// <param name="clickPosition">The coordinate of the object on which the click occurs.</param>
        protected internal void Execute(int itemLevel, MapCoordinate clickPosition);

        /// <summary>
        /// Canceling the last command.
        /// It can be called no more than once after calling Execute.
        /// </summary>
        protected internal void Undo();

        /// <summary>
        /// Filters commands by type T. See <see cref="FilterFactory"/> class for creating a filter.
        /// </summary>
        /// <typeparam name="T">Type of click command.</typeparam>
        /// <returns>
        /// Should return a list of references to all internal commands
        /// (if the current command combines several at once), including itself,
        /// whose type is T. If there are no such commands, it returns an empty list.
        /// </returns>
        protected internal IEnumerable<T> Filter<T>() where T : IClickCommand;
    }
}
