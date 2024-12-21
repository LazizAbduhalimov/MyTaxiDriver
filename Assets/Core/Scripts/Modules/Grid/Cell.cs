using Client.Game;
using UnityEngine;

namespace LGrid
{
    public class Cell
    {
        public Vector3Int Position;
        public TaxiBase TaxiBase;

        public Cell(Vector3Int position)
        {
            Position = position;
        }
    }
}