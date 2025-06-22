using System;
using Esper.ESave.SavableObjects;

namespace Game.Saving
{
    [Serializable]
    public class CellsData
    {
        public SavableVector CellPositions;
        public int TaxiLevel;
    }
}