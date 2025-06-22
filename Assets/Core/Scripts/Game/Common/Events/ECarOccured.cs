using Game.Game;

namespace Game
{
    public struct ECarOccured
    {
        public TaxiMb TaxiMb;

        public void Invoke(TaxiMb taxiMb)
        {
            TaxiMb = taxiMb;
        }
    }
}