using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Module.Bank
{
    public class BankSystem : IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsFilterInject<Inc<EAddToBank>> _eAddToBankFilter = "events";
        private EcsFilterInject<Inc<ESpendFromBank>> _eSpendFromBankFilter = "events";
        
        private EcsPoolInject<CBank> _cBank;
        private EcsPoolInject<EBankValueChanged> _eBankValueChanged;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eAddToBankFilter.Value)
            {
                ref var data = ref _eAddToBankFilter.Pools.Inc1.Get(entity);
                if (data.Value < 0)
                    throw new ArgumentException("Negative number when tried to add");

                if (data.BankEntity.Unpack(_world.Value, out var bankEntity))
                {
                    ref var bank = ref _cBank.Value.Get(bankEntity);
                    var oldValue = bank.Value;
                    bank.Value += data.Value;
                    _eBankValueChanged.NewEntity(out _).Invoke(data.BankEntity, oldValue, bank.Value);
                }
                _eAddToBankFilter.Pools.Inc1.Del(entity);
            }

            foreach (var entity in _eSpendFromBankFilter.Value)
            {
                ref var data = ref _eSpendFromBankFilter.Pools.Inc1.Get(entity);
                if (data.Value > 0)
                    throw new ArgumentException("Positive number when tried to subtract");

                if (data.BankEntity.Unpack(_world.Value, out var bankEntity))
                {
                    ref var bank = ref _cBank.Value.Get(bankEntity);
                    var oldValue = bank.Value;
                    bank.Value -= data.Value;
                    _eBankValueChanged.NewEntity(out _).Invoke(data.BankEntity, oldValue, bank.Value);
                }
                
                _eSpendFromBankFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}