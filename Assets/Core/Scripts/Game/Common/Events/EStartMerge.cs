using Game.Game;

namespace Game
{
    public struct EStartMerge
    {
        public TaxiMb Source;
        public TaxiMb Target;

        public void Invoke(TaxiMb source, TaxiMb target)
        {
            Source = source;
            Target = target;
        }
    }
}