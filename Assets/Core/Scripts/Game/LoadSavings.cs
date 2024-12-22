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
        public int CoinsGainedIfZero;
        private SaveFileSetup _setup;
        
        public void Start()
        {
            _setup = GetComponent<SaveFileSetup>();
            var file = _setup.GetSaveFile();
            LoadCellsData(file);
            LoadCoins(file);
            Debug.Log("Loaded!");
        }

        private static void LoadCellsData(SaveFile file)
        {
            if (file.HasData("CellNumber"))
            {
                var cellNumber = file.GetData<int>("CellNumber");
                for (var i = 0; i < cellNumber; i++)
                {
                    var cellsData = file.GetData<CellsData>($"Cell{i}");
                    var position = cellsData.CellPositions.vector3Value;
                    var level = cellsData.TaxiLevel;
                    if (Map.Instance.IsCellExists(position, out var cell))
                    {
                        var pool = AllVehicles.Instance.CarsPool[level - 1];
                        var poolObject = pool.GetFromPool(position);
                        cell.TaxiBase = poolObject.GetComponent<TaxiBase>();
                    }
                }
            }
        }

        private void LoadCoins(SaveFile file)
        {
            if (file.HasData("Coins"))
            {
                Bank.SpendCoins(this, Bank.Coins);
                Bank.AddCoins(this, file.GetData<int>("Coins"));
            }
            else
            {
                Bank.AddCoins(this, CoinsGainedIfZero);
            }
        }
    }
}