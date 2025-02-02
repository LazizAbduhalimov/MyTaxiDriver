using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Module.Bank;
using UnityEngine;

namespace Client.Game
{
    public class CoinDisplaySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPoolInject<CCoinDisplayer> _cCoinDisplayer;
        private EcsFilterInject<Inc<CCoinDisplayer>> _cCoinDisplayerFilter;
        private EcsFilterInject<Inc<EBankValueChanged>> _eBankValueChagedFilter = "events";
        
        public void Init(IEcsSystems systems)
        {
            var displayer = Object.FindObjectOfType<CoinShower>();
            _cCoinDisplayer.NewEntity(out _).Invoke(displayer.Text);
            ChangeCoins();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBankValueChagedFilter.Value)
            {
                ChangeCoins();
            }
        }

        private void ChangeCoins()
        {
            foreach (var displayerEntity in _cCoinDisplayerFilter.Value)
            {
                ref var displayer = ref _cCoinDisplayerFilter.Pools.Inc1.Get(displayerEntity);
                displayer.Text.text = Bank.Coins.ToString();
            }
        }
    }
}