using UnityEngine;

namespace Client.Game.Test
{
    public struct CDragObject
    {
        public DragAndDropMb DragAndDropMb;

        public void Invoke(DragAndDropMb dragAndDropMb)
        {
            DragAndDropMb = dragAndDropMb;
        }
    }
}