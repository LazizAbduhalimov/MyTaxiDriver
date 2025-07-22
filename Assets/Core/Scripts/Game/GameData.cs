using LGrid;
using PoolSystem.Alternative;
using UnityEngine;

namespace Game
{
    public class GameData
    {
        public static GameData Instance { get; private set; }
        public int VehicleDefaultCost { get; private set; } = 5;
        public int PurchaseNumber { get; set; } = 1;
        public BonusesData BonusesData { get; private set; }
        
        public Map Map;
        public PoolService PoolService;
        public AllPools AllPools;
        public Postponer Postponer;
        
        public GameData()
        {
            Instance = this;
            Map = new Map();
            BonusesData = new BonusesData();
            AllPools = Object.FindObjectOfType<AllPools>();
            PoolService = new PoolService("Pools");
            Postponer = new Postponer(CommonUtilities.EventsWorld);
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

    public class BonusesData
    {
        public float SpeedBoostDuration => 12.5f;
        public float DoubledCoinsDuration => 17.5f;
    }
}