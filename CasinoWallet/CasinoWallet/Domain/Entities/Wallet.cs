namespace CasinoWallet.Domain.Entities;

/// <summary>
/// Represents a player's wallet that holds and manages their balance.
/// </summary>
public class Wallet
{
    /// <summary>
    /// Gets the current balance of the wallet.
    /// </summary>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Adds funds to the wallet.
    /// </summary>
    /// <param name="amount">The amount to deposit (must be positive).</param>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        Balance += amount;
    }

    /// <summary>
    /// Withdraws funds from the wallet.
    /// </summary>
    /// <param name="amount">The amount to withdraw (must be positive and not exceed the current balance).</param>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown if there are insufficient funds.</exception>
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
    }

    /// <summary>
    /// Applies a bet by deducting the bet amount from the balance.
    /// </summary>
    /// <param name="amount">The amount to bet (must be valid and less than or equal to balance).</param>
    public void ApplyBet(decimal amount)
    {
        Withdraw(amount); // reuse logic
    }

    /// <summary>
    /// Applies a win by adding the win amount to the balance.
    /// </summary>
    /// <param name="amount">The amount won (can be zero or positive).</param>
    /// <exception cref="ArgumentException">Thrown if the win amount is negative.</exception>
    public void ApplyWin(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Win amount must be non-negative.", nameof(amount));

        Balance += amount;
    }
}