using Client.Game;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Car")]
public class CarsConfig : ScriptableObject
{
    public Vector2 SpeedBoundries;
    public AnimationCurve Speed;
    
    [Space]
    public Vector2 ProfitBoundries;
    public AnimationCurve Profit;
    
    [Space]
    public Vector2 CostBoundries;
    public AnimationCurve Cost;

    public TaxiBase[] Cars;

    public float GetSpeed(int level)
    {
        var t = Speed.Evaluate((level-1) / 8f);
        return Mathf.Lerp(SpeedBoundries.x, SpeedBoundries.y, t);
    }
    
    public float GetProfit(int level)
    {
        var t = Profit.Evaluate((level-1) / 8f);
        return Mathf.Lerp(ProfitBoundries.x, ProfitBoundries.y, t);
    }

    [ContextMenu("Configurate")]
    public void Configurate()
    {
        foreach (var taxiBase in Cars)
        {
            taxiBase.Configurate(this);
        }
    }
}
