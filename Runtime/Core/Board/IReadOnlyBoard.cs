using System;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Immutable game board.
    /// </summary>
    public interface IReadOnlyBoard : IShape
    {
        /// <summary>
        /// Invoked when the <c>Add</c>, <c>Remove</c>, <c>Open</c> methods are Invoked.
        /// </summary>
        /// <remarks>
        /// Don't abuse
        /// </remarks>
        event Action Updated;

        /// <summary>
        /// Collection of open cells.
        /// Does not intersect with a set of contour cells.
        /// </summary>
        IReadOnlyCollection<MapCoordinate> OpenedCollection { get; }

        /// <summary>
        /// Collection of contour cells.
        /// Does not intersect with a set of opened cells.
        /// </summary>
        IReadOnlyCollection<MapCoordinate> ContourCollection { get; }

        /// <summary>
        /// provides a method for getting an item on the board by coordinate.
        /// </summary>
        /// <param name="coordinate">Target coordinate.</param>
        Item Item(MapCoordinate coordinate);

        /// <summary>
        /// Checks if there is an object on the board at the specified coordinate.
        /// </summary>
        /// <param name="coordinate">Target coordinate.</param>
        /// <returns>true if there is an item for the specified position. false if not.</returns>
        bool HasItem(MapCoordinate coordinate);

        /// <summary>
        /// Checks whether the specified position belongs
        /// to the collection of opened positions.
        /// </summary>
        /// <param name="coordinate">The position to be checked.</param>
        /// <returns>true, if the position belongs to a
        /// collection of opened positions. false if not.</returns>
        bool Opened(MapCoordinate coordinate);

        /// <summary>
        /// Checks whether the specified position belongs
        /// to the collection of contour positions.
        /// </summary>
        /// <param name="coordinate">The position to be checked.</param>
        /// <returns>true if the position belongs to a
        /// collection of contour positions. false if not.</returns>
        bool Contour(MapCoordinate coordinate);
    }
}
