using System;
using Esper.ESave.SavableObjects;

namespace Client.Game.Save
{
    [Serializable]
    public class CellsData
    {
        public SavableVector CellPositions;
        public int TaxiLevel;
    }
}