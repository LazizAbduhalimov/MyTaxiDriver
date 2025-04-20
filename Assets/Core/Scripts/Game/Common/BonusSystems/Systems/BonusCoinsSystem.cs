using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class BonusCoinsSystem : IEcsRunSystem
    {
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBonusCoins>> _eBonusCoinsSystem = "event";
        
        private float _bonusScaler = 1.5f;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBonusCoinsSystem.Value)
            {
                var vehiclePrice = _gameData.Value.GetVehicleCost() * _bonusScaler;
                var bonus = Mathf.RoundToInt(vehiclePrice);
                Bank.AddCoins(this, bonus);
                _eBonusCoinsSystem.Pools.Inc1.Del(entity);
            }
        }
    }
}