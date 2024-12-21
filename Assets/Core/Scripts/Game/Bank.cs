using System;

public static class Bank
{
    public static int Coins { get; private set; }

    public static Action<object, int, int> OnCoinsValueChangedEvent;

    public static void AddCoins(object sender, int coins)
    {
        if (coins < 1)
            throw new ArgumentException("Number of coins should be positive");
        
        var oldValue = Coins;
        Coins += coins;
        OnCoinsValueChangedEvent?.Invoke(sender, oldValue, Coins);
    }

    public static bool SpendCoins(object sender, int coins)
    {
        if (coins < 1)
            throw new ArgumentException("Number of coins should be positive");

        if (!HasEnoughCoins(coins))
            return false;

        var oldValue = Coins;
        Coins -= coins;
        OnCoinsValueChangedEvent?.Invoke(sender, oldValue, Coins);
        return true;
    }

    public static bool HasEnoughCoins(int number)
    {
        return Coins >= number;
    }
}
