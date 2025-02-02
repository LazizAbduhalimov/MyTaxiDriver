using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    public class DragAndDropMb : MonoBehaviour
    {
        public bool Enabled { get; private set; } = true;
        public EcsPackedEntity PackedEntity;

        private void OnMouseDown()
        {
            if (!Enabled) return;
            CommonUtilities.EventsWorld.GetPool<EDragStart>().NewEntity(out _).Invoke(PackedEntity);
        }

        private void OnMouseUp()
        {
            if (!Enabled) return;
            CommonUtilities.EventsWorld.GetPool<EDragEnd>().NewEntity(out _).Invoke(PackedEntity);
        }

        public void SetEnabled(bool isEnabled)
        {
            Enabled = isEnabled;
        }
    }
}