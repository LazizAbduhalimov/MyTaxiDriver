using Leopotam.EcsLite;

namespace Client
{
    public struct EDragStart
    {
        public EcsPackedEntity PackedEntity;
        
        public void Invoke(EcsPackedEntity packedEntity)
        {
            PackedEntity = packedEntity;
        }
    }
}