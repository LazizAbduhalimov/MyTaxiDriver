using Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace YG.Systems
{
    public class LoadYG2DataSystem : IEcsInitSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<AllPools> _allPools;
        private EcsPoolInject<CActive> _cActive;

        public void Init(IEcsSystems systems)
        {
            LoadCellsData();
            LoadCoins();
            LoadCost();
        }

        private void LoadCost()
        {
            _gameData.Value.PurchaseNumber = YG2.saves.Cost;
        }

        private void LoadCellsData()
        {
            foreach (var cellData in YG2.saves.CellsData)
            {
                var position = cellData.Position;
                var level = cellData.TaxiLevel;
                if (_map.Value.IsCellExists(position, out var cell))
                {
                    var pool = _allPools.Value.CarsPool[level - 1];
                    var car = pool.GetFromPool(position);
                    car.Drive();
                    cell.IsOccupied = true;
                    _cActive.Value.Add(car.PackedEntity.FastUnpack());
                    // Debug.Log("Cell loaded!");
                }
            }
        }

        private void LoadCoins()
        {
            Bank.SetCoins(this, YG2.saves.Coins);
        }
    }
}