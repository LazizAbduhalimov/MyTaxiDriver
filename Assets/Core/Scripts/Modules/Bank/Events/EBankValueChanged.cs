using Leopotam.EcsLite;

namespace Module.Bank
{
    public struct EBankValueChanged
    {
        public EcsPackedEntity PackedEntity;
        public long OldValue;
        public long NewValue;

        public void Invoke(EcsPackedEntity packedEntity, long oldValue, long newValue)
        {
            PackedEntity = packedEntity;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}