using System;
using UnityEngine;

namespace Client.Game
{
    public class Collector : MonoBehaviour
    {
        [HideInInspector] public TaxiBase TaxiBase;

        private void Start()
        {
            TaxiBase = GetComponentInParent<TaxiBase>();
        }
    }
}