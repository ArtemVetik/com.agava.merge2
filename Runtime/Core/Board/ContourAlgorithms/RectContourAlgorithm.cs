using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    /// <summary>
    /// Contour calculation algorithm that selects 4
    /// adjacent positions (left, right, top, bottom).
    /// </summary>
    public class RectContourAlgorithm : IContourAlgorithm
    {
        public IEnumerable<MapCoordinate> Compute(IEnumerable<MapCoordinate> openPositions)
        {
            var result = new HashSet<MapCoordinate>();
            var right = new MapCoordinate(1, 0);
            var down = new MapCoordinate(0, 1);

            foreach (var coordinate in openPositions)
            {
                if (coordinate.X < int.MaxValue)
                    result.Add(coordinate + right);
                if (coordinate.Y < int.MaxValue)
                    result.Add(coordinate + down);
                if (coordinate.X > 0)
                    result.Add(coordinate - right);
                if (coordinate.Y > 0)
                    result.Add(coordinate - down);
            }

            result.ExceptWith(openPositions);
            return result;
        }
    }
}
