using System;
using Client.Game.Save;
using Esper.ESave;
using LGrid;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class LoadSavings : MonoBehaviour
    {
        public long CoinsGainedIfZero;
        private SaveFileSetup _setup;
        
        public void Init()
        {
            _setup = GetComponent<SaveFileSetup>();
            var file = _setup.GetSaveFile();
            LoadCellsData(file);
            LoadCoins(file);
            LoadCost(file);
            file.Save();
            Debug.Log("Loaded!");
        }

        private void LoadCost(SaveFile file)
        {
            if (file.HasData("PurchaseNumber"))
            {
                Links.Instance.VehicleBuyer.PurchaseNumber = file.GetData<int>("PurchaseNumber");
                Links.Instance.VehicleBuyer.ChangeCostText();
            }
        }

        private static void LoadCellsData(SaveFile file)
        {
            string text = "CellNumber";
            if (file.HasData(text))
            {
                var cellNumber = file.GetData<int>(text);
                for (var i = 0; i < cellNumber; i++)
                {
                    var cellsData = file.GetData<CellsData>($"Cell{i}");
                    var position = cellsData.CellPositions.vector3Value;
                    var level = cellsData.TaxiLevel;
                    if (Map.Instance.IsCellExists(position, out var cell))
                    {
                        var pool = AllVehicles.Instance.CarsPool[level - 1];
                        pool.GetFromPool(position);
                        Debug.Log("Cell loaded!");
                    }
                    file.DeleteData($"Cell{i}");
                }
                file.DeleteData(text);
            }
        }

        private void LoadCoins(SaveFile file)
        {
            if (file.HasData("Coins"))
            {
                var number = file.GetData<long>("Coins");
                Bank.SetCoins(this, number);
                Debug.Log("Coins loaded!");
            }
            else
            {
                Bank.AddCoins(this, CoinsGainedIfZero);
            }
        }
    }
}