using Client.Game;

namespace Client
{
    public struct CSpeedBooster
    {
        public SpeedBoosterMb SpeedBoosterMb;

        public void Invoke(SpeedBoosterMb speedBoosterMb)
        {
            SpeedBoosterMb = speedBoosterMb;
        }
    }
}