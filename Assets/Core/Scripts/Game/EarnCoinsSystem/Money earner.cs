using Client.Game;
using Core.Scripts.Game;
using UnityEngine;

public class Moneyearner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collector>(out var collector))
        {
            var world = Utilities.EventsWorld;
            world.GetPool<EEarnMoney>().Add(world.NewEntity()).Invoke(collector);
        }
    }
}
