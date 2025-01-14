namespace Client.Game
{
    public struct EEarnMoney
    {
        public Collector Collector;

        public void Invoke(Collector collector)
        {
            Collector = collector;
        }
    }
}