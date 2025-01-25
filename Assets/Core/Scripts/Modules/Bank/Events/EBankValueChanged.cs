namespace Module.Bank
{
    public struct EBankValueChanged
    {
        public long OldValue;
        public long NewValue;

        public void Invoke(long oldValue, long newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}