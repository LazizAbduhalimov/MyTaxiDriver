using LGrid;
using PoolSystem.Alternative;
using UnityEngine;

namespace Client
{
    public class GameData
    {
        public static GameData Instance { get; private set; }
        public int VehicleDefaultCost { get; private set; } = 5;
        public int PurchaseNumber { get; set; } = 1;
        
        public Map Map;
        public PoolService PoolService;
        public AllPools AllPools;
        
        public GameData()
        {
            Instance = this;
            Map = new Map();
            AllPools = Object.FindObjectOfType<AllPools>();
            PoolService = new PoolService("Pools");
        }
    }

    public static class GameDataExtensions
    {
        public static int GetVehicleCost(this GameData gameData)
        {
            return gameData.VehicleDefaultCost * gameData.PurchaseNumber * gameData.PurchaseNumber;
        }

        public static int GetBuyingCarLevel(this GameData gameData)
        {
            return gameData.PurchaseNumber / 63;
        } 
    }
}