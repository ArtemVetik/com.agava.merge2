using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// It is a set of MapCoordinate for specifying the shape of a two-dimensional grid.
    /// </summary>
    public interface IShape
    {
        /// <summary>
        /// Shape bounds width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Shape bounds height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Checks if this shape contains the position.
        /// </summary>
        /// <param name="position">Position to check for containment.</param>
        /// <returns>true if position contained; false if not.</returns>
        bool Contains(MapCoordinate position);

        /// <summary>
        /// Check if this shape is a superset of other.
        /// </summary>
        /// <param name="other">Set of coordinates.</param>
        /// <returns>true if this shape is a superset of other; false if not.</returns>
        bool IsSupersetOf(IEnumerable<MapCoordinate> other);
    }
}
