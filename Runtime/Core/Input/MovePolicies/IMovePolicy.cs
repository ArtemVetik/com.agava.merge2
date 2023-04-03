
namespace Agava.Merge2.Core
{
    public interface IMovePolicy
    {
        bool CanMove(MapCoordinate fromPosition, MapCoordinate toPosition);
        void Move(MapCoordinate fromPosition, MapCoordinate toPosition);
    }
}
