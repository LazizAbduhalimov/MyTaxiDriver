using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game
{
    public class BonusCoinsSystem : IEcsRunSystem
    {
        private EcsCustomInject<GameData> _gameData;
        private EcsFilterInject<Inc<EBonusCoins>> _eBonusCoinsSystem = "events";
        private EcsPoolInject<EDisplayFloatingCoin> _eDisplayCoins = "events";
        
        private Vector2Int _bonusScaler = new (2, 5);
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBonusCoinsSystem.Value)
            {
                var vehiclePrice = _gameData.Value.GetVehicleCost() * Random.Range(_bonusScaler.x, _bonusScaler.y);
                var bonus = Mathf.Clamp(Mathf.RoundToInt(vehiclePrice), 1500, int.MaxValue);
                Bank.AddCoins(this, bonus);
                _eDisplayCoins.NewEntity(out _).Invoke(Vector3.up * 2, bonus, 1.5f);
                _eBonusCoinsSystem.Pools.Inc1.Del(entity);
            }
        }
    }
}