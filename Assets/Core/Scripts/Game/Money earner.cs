using Client.Game;
using UnityEngine;

public class Moneyearner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collector>(out var taxi))
        {
            taxi.TaxiBase.Earn();
        }
    }
}
