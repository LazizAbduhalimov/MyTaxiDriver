using Esper.ESave;
using LGrid;
using Module.Bank;
using UnityEngine;

namespace Client.Saving
{
    public class SaveFileSetupMb : MonoBehaviour
    {
        public long CoinsGainedIfZero;
        public SaveFileSetup File;
        
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (GameData.Instance == null) return;
            SaveGame();
        }
        
        public void Drop()
        {
            CommonUtilities.EventsWorld.GetPool<EDropData>().NewEntity(out _);
        }
        
        private void SaveGame()
        {
            var saveFile = File.GetSaveFile();
            SaveCells(saveFile);
            SaveCoins(saveFile);
            SaveCost(saveFile);
            saveFile.Save();
            Debug.Log("Игра сохранена!");
        }

        private void SaveCost(SaveFile saveFile)
        {
            saveFile.AddOrUpdateData("PurchaseNumber", GameData.Instance.PurchaseNumber);
        }

        private void SaveCoins(SaveFile saveFile)
        {
            saveFile.AddOrUpdateData("Coins", Bank.Coins);
        }
        
        private void SaveCells(SaveFile saveFile)
        {
            var i = 0;
            foreach (var pair in GameData.Instance.Map.Cells)
            {
                if (!MapUtils.TryGetCellOccupier<CTaxi, CActive>(pair.Key, CommonUtilities.World, out var taxi)) continue;
                var cellSaveData = new CellsData
                {
                    CellPositions = ((Vector3)pair.Key).ToSavable(),
                    TaxiLevel = taxi.TaxiMb.Level
                };
                var id = $"Cell{i}";
                saveFile.AddOrUpdateData(id, cellSaveData);
                i++;
            }
            saveFile.AddOrUpdateData("CellNumber", i);
        }
    }
}