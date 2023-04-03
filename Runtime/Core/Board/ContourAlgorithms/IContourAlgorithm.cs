using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Algorithm for contour calculation.
    /// </summary>
    public interface IContourAlgorithm
    {
        /// <summary>
        /// Calculates all contour positions.
        /// </summary>
        /// <param name="openPositions">Collection of positions 
        /// for which the contour should be calculated.</param>
        /// <returns>Collection of contour positions.</returns>
        IEnumerable<MapCoordinate> Compute(IEnumerable<MapCoordinate> openPositions);
    }
}
