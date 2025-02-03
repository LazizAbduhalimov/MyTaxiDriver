using System;
using Esper.ESave.SavableObjects;

namespace Client.Saving
{
    [Serializable]
    public class CellsData
    {
        public SavableVector CellPositions;
        public int TaxiLevel;
    }
}