using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace Client
{
    public class BonusCarSystem : IEcsRunSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<AllPools> _allPools;
        private EcsPoolInject<CActive> _cActive;
        private EcsFilterInject<Inc<EBonusCar>> _eBonusCar = "events";
        
        private int CarLevel => _gameData.Value.GetBuyingCarLevel();

        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eBonusCar.Value)
            {
                if (!_map.Value.HasFreeCell(out var pair))
                {
                    continue;
                }
                var bonusCarLevel = Mathf.Clamp(CarLevel, 1, 7);
                var taxiMb = _allPools.Value.CarsPool[bonusCarLevel].GetFromPool(pair.Key);
                var taxiEntity = taxiMb.PackedEntity.FastUnpack();
                taxiMb.Drive();
                pair.Value.IsOccupied = true;
                _cActive.Value.Add(taxiEntity);
                _eBonusCar.Pools.Inc1.Del(entity);
            }
        }
    }
}