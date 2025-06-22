namespace Game.Saving
{
    public struct CSaveFileSetup
    {
        public SaveFileSetupMb SaveFileSetupMb;

        public void Invoke(SaveFileSetupMb saveFileSetupMb)
        {
            SaveFileSetupMb = saveFileSetupMb;
        }
    }
}