using Game.Game;

namespace Game
{
    public struct EMerged
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