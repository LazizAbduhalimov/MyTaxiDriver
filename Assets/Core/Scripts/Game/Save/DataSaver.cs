using System;
using System.Collections.Generic;
using System.Linq;
using Esper.ESave;
using LGrid;
using UnityEngine;

namespace Client.Game.Save
{
    public class DataSaver : MonoBehaviour
    {
        private SaveFileSetup _saveFileSetup;

        private void Start()
        {
            _saveFileSetup = GetComponent<SaveFileSetup>();
        }

        void OnApplicationQuit()
        {
            SaveGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveGame();
        }

        private void SaveGame()
        {
            if (_saveFileSetup == null) return;
            var saveFile = _saveFileSetup.GetSaveFile();
            SaveCells(saveFile);
            SaveCoins(saveFile);
            // SaveCost(saveFile);
            saveFile.Save();
            Debug.Log("Игра сохранена!");
        }

        // private void SaveCost(SaveFile saveFile)
        // {
        //     saveFile.AddOrUpdateData("PurchaseNumber", Links.Instance.VehicleBuyer.PurchaseNumber);
        // }

        private void SaveCoins(SaveFile saveFile)
        {
            saveFile.AddOrUpdateData("Coins", Bank.Coins);
        }
        
        private void SaveCells(SaveFile saveFile)
        {
            var i = 0;
            foreach (var pair in Map.Instance.Cells)
            {
                if (pair.Value.TaxiBase == null) continue;
                var cellSaveData = new CellsData
                {
                    CellPositions = ((Vector3)pair.Key).ToSavable(),
                    TaxiLevel = pair.Value.TaxiBase.Level
                };
                var id = $"Cell{i}";
                saveFile.AddOrUpdateData(id, cellSaveData);
                i++;
            }
            saveFile.AddOrUpdateData("CellNumber", i);
        }
    }
}