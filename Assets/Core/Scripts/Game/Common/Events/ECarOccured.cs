using Client.Game;

namespace Client
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