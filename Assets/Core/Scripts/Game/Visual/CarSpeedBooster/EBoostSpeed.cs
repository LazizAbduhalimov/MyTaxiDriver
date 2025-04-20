using Client.Game;

namespace Client
{
    public struct EBoostSpeed
    {
        public SpeedBoosterMb SpeedBoosterMb;
        public float BoostTime;

        public void Invoke(SpeedBoosterMb speedBoosterMb, float boostTime = 1f)
        {
            SpeedBoosterMb = speedBoosterMb;
            BoostTime = boostTime;
        }
    }
}