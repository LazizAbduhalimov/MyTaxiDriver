using Game.Game;

namespace Game
{
    public struct EBoostSpeed
    {
        public SpeedBoosterMb SpeedBoosterMb;
        public float BoostTime;

        public void Invoke(SpeedBoosterMb speedBoosterMb, float boostTime = 1.5f)
        {
            SpeedBoosterMb = speedBoosterMb;
            BoostTime = boostTime;
        }
    }
}