namespace Client
{
    public class GameData
    {
        public int VehicleDefaultCost { get; private set; } = 5;
        public int PurchaseNumber { get; set; } = 1;
    }

    public static class GameDataExtensions
    {
        public static int GetVehicleCost(this GameData gameData)
        {
            return gameData.VehicleDefaultCost * gameData.PurchaseNumber * gameData.PurchaseNumber;
        }

        public static int GetBuyingCarLevel(this GameData gameData)
        {
            return gameData.PurchaseNumber >= 63 ? 1 : 0;
        } 
    }
}