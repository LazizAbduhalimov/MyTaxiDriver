using Game.Game;
using LGrid;
using UnityEngine;

namespace Game
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