using Client.Game;
using UnityEngine;

public class Moneyearner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TaxiBase>(out var taxi))
        {
            taxi.Earn();
        }
    }
}
