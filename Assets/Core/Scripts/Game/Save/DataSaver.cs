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

        void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                SaveGame();
            }
        }

        private void SaveGame()
        {
            SaveCoins();
            SaveCells();
        }

        private void SaveCoins()
        {
            var saveFile = _saveFileSetup.GetSaveFile();
            saveFile.AddOrUpdateData("Coins", Bank.Coins);
            saveFile.Save();
        }
        
        private void SaveCells()
        {
            var saveFile = _saveFileSetup.GetSaveFile();
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
            saveFile.Save();
            Debug.Log("Игра сохранена!");
        }
    }
}