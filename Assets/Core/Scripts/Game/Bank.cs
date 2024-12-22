using System;

public static class Bank
{
    public static long Coins { get; private set; }

    public static Action<object, long, long> OnCoinsValueChangedEvent;

    public static void SetCoins(object sender, long coins)
    {
        if (coins < 0) throw new ArgumentException("Negative number");
        var oldValue = Coins;
        Coins = coins;
        OnCoinsValueChangedEvent.Invoke(sender, oldValue, Coins);
    }

    public static void AddCoins(object sender, long coins)
    {
        if (coins < 1)
            throw new ArgumentException("Number of coins should be positive");
        
        var oldValue = Coins;
        Coins += coins;
        OnCoinsValueChangedEvent?.Invoke(sender, oldValue, Coins);
    }

    public static bool SpendCoins(object sender, long coins)
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

    public static bool HasEnoughCoins(long number)
    {
        return Coins >= number;
    }
}
