using Client.Game;

namespace Client
{
    public struct EBoostSpeed
    {
        public SpeedBoosterMb SpeedBoosterMb;

        public void Invoke(SpeedBoosterMb speedBoosterMb)
        {
            SpeedBoosterMb = speedBoosterMb;
        }
    }
}