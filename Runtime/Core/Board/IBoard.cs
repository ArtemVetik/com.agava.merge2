
namespace Agava.Merge2.Core
{
    /// <summary>
    /// Mutable game board
    /// </summary>
    public interface IBoard : IReadOnlyBoard
    {
        void Add(Item item, MapCoordinate coordinate);
        void Remove(MapCoordinate coordinate);
        void Open(MapCoordinate coordinate);
    }
}
