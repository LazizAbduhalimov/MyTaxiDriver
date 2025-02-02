using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    public class DragAndDropMb : MonoBehaviour
    {
        public EcsPackedEntity PackedEntity;

        private void OnMouseDown()
        {
            Utilities.EventsWorld.GetPool<EDragStart>().NewEntity(out _).Invoke(PackedEntity);
        }

        private void OnMouseUp()
        {
            Utilities.EventsWorld.GetPool<EDragEnd>().NewEntity(out _).Invoke(PackedEntity);
        }
    }
}