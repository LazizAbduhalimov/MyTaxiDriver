using Leopotam.EcsLite;
using UnityEngine;

namespace Client
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