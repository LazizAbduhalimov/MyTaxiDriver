using Client;
using Client.Game;
using Client.Game.Test;

namespace Core.Scripts.Game
{
    public static class Utilities
    {
        public static DragAndDropMb GetDragAndDropMb(this TaxiMb taxiMb)
        {
            var entity = taxiMb.PackedEntity.FastUnpack();
            ref var dragObject = ref CommonUtilities.World.GetPool<CDragObject>().Get(entity);
            return dragObject.DragAndDropMb;
        }
        
        public static TaxiMb GetDragAndDropMb(this DragAndDropMb taxiMb)
        {
            var entity = taxiMb.PackedEntity.FastUnpack();
            ref var cTaxi = ref CommonUtilities.World.GetPool<CTaxi>().Get(entity);
            return cTaxi.TaxiMb;
        }
    }
}