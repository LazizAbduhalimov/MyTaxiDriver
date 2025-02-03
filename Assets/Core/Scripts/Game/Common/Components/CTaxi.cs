using Client.Game;
using LGrid;
using UnityEngine;

namespace Client
{
    public struct CTaxi : ICellStandable
    {
        public TaxiMb TaxiMb;
        public Vector3 Coords => TaxiMb.transform.position;

        public void Invoke(TaxiMb taxiMb)
        {
            TaxiMb = taxiMb;
        }
    }
}