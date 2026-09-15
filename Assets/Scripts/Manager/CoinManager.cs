using System;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [Header("Coin")]
    [SerializeField] private int coins;

    public event Action<int> CoinsChanged;

    public int Coins => this.coins;

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        this.coins += amount;
        this.CoinsChanged?.Invoke(this.coins);
    }

    public void SetCoins(int amount)
    {
        this.coins = Mathf.Max(0, amount);
        this.CoinsChanged?.Invoke(this.coins);
    }
}