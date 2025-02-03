using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client.Game
{
    public class Collector : MonoBehaviour
    {
        [FormerlySerializedAs("TaxiBase")] [HideInInspector] public TaxiMb TaxiMb;

        private void Start()
        {
            TaxiMb = GetComponentInParent<TaxiMb>();
        }
    }
}