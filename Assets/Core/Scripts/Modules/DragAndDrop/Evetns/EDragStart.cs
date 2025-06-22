using Leopotam.EcsLite;

namespace Game
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