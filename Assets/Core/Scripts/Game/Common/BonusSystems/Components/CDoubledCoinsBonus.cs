namespace Client
{
    public struct CDoubledCoinsBonus
    {
        public float PassedTime;
        public float Duration;

        public void Invoke(float duration)
        {
            PassedTime = 0;
            Duration = duration;
        }
    }
}