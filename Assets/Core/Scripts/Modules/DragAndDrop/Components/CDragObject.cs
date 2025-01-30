using UnityEngine;

namespace Client.Game.Test
{
    public struct CDragObject
    {
        public DragAndDropMb DragAndDropMb;
        public Vector3 LastDragInitialPoint;

        public void Invoke(DragAndDropMb dragAndDropMb)
        {
            DragAndDropMb = dragAndDropMb;
            LastDragInitialPoint = dragAndDropMb.transform.position;
        }
    }
}