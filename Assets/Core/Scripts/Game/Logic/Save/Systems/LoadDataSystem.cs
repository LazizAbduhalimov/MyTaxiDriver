using Esper.ESave;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine;

namespace Client.Saving
{
    public class LoadDataSystem : IEcsInitSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<AllPools> _allPools;
        private EcsPoolInject<CSaveFileSetup> _cSaveFileSetup;
        private EcsPoolInject<CActive> _cActive;

        private const string Text = "CellNumber";
        
        public void Init(IEcsSystems systems)
        {
            var setup = Object.FindObjectOfType<SaveFileSetupMb>();
            _cSaveFileSetup.NewEntity(out _).Invoke(setup);
            var file = setup.File.GetSaveFile();
            LoadCellsData(file);
            LoadCoins(file, setup.CoinsGainedIfZero);
            LoadCost(file);
            file.Save();
            Debug.Log("Loaded!");
        }

        private void LoadCost(SaveFile file)
        {
            if (file.HasData("PurchaseNumber"))
            {
                _gameData.Value.PurchaseNumber = file.GetData<int>("PurchaseNumber");
            }
        }

        private void LoadCellsData(SaveFile file)
        {
            if (!file.HasData(Text)) return;
            var cellNumber = file.GetData<int>(Text);
            for (var i = 0; i < cellNumber; i++)
            {
                var cellsData = file.GetData<CellsData>($"Cell{i}");
                var position = cellsData.CellPositions.vector3Value;
                var level = cellsData.TaxiLevel;
                if (_map.Value.IsCellExists(position, out var cell))
                {
                    var pool = _allPools.Value.CarsPool[level - 1];
                    var car = pool.GetFromPool(position);
                    car.Drive();
                    cell.IsOccupied = true;
                    _cActive.Value.Add(car.PackedEntity.FastUnpack());
                    Debug.Log("Cell loaded!");
                }
                file.DeleteData($"Cell{i}");
            }
            file.DeleteData(Text);
        }

        private void LoadCoins(SaveFile file, long coinsGainedIfZero)
        {
            var coins = file.HasData("Coins") ? file.GetData<long>("Coins") : coinsGainedIfZero;
            Bank.SetCoins(this, coins);
        }
    }
}