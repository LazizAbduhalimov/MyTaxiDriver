using Leopotam.EcsLite;
using UnityEngine;

namespace Game
{
    public struct EDragEnd
    {
        public EcsPackedEntity PackedEntity;
        
        public void Invoke(EcsPackedEntity packedEntity)
        {
            PackedEntity = packedEntity;
        }
    }
}