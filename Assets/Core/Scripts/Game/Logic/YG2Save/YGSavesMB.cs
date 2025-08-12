using System;
using System.Collections.Generic;
using Core.Data;
using Game;
using Game.Saving;
using LGrid;
using UnityEngine;

namespace YG
{
    public class YGSavesMB : MonoBehaviour
    {
        private void OnEnable()
        {
            YG2.onHideWindowGame += SaveGame;
        }

        private void OnDisable()
        {
            YG2.onHideWindowGame -= SaveGame;
        }

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
            SaveCellsYG2();
            SaveCoinsYG2();
            SaveCostYG2();
            Debug.Log("Игра сохранена на YG2!");
            YG2.SaveProgress();
        }

        private void SaveCellsYG2()
        {
            var cellsData = new List<CellData>();
            foreach (var pair in GameData.Instance.Map.Cells)
            {
                if (!MapUtils.TryGetCellOccupier<CTaxi>(pair.Key, CommonUtilities.World, out var taxi)) continue;
                var cellSaveData = new CellData
                {
                    Position = pair.Key,
                    TaxiLevel = taxi.TaxiMb.Level
                };
                cellsData.Add(cellSaveData);
            }

            YG2.saves.CellsData = cellsData;
        }

        private void SaveCoinsYG2()
        {
            YG2.saves.Coins = Bank.Coins;
        }

        private void SaveCostYG2()
        {
            YG2.saves.Cost = GameData.Instance.PurchaseNumber;
        }
    }
}