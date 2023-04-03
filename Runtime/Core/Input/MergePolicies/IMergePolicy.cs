
namespace Agava.Merge2.Core
{
    public interface IMergePolicy
    {
        bool CanMerge(MapCoordinate firstItemCoordinate, MapCoordinate secondItemCoordinate);
        void Merge(MapCoordinate firstItemCoordinate, MapCoordinate secondItemCoordinate);
    }
}
