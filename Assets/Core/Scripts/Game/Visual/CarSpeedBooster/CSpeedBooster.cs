using Game.Game;

namespace Game
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