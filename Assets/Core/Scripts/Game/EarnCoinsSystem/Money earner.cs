using Client;
using Client.Game;
using UnityEngine;

public class Moneyearner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collector>(out var collector))
        {
            var world = CommonUtilities.EventsWorld;
            world.GetPool<EEarnMoney>().Add(world.NewEntity()).Invoke(collector);
        }
    }
}
