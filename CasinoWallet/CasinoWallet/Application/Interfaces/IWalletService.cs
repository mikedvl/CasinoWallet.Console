namespace CasinoWallet.Application.Interfaces;

/// <summary>
/// Represents the contract for wallet operations such as deposits, withdrawals, and bet handling.
/// </summary>
public interface IWalletService
{
    /// <summary>
    /// Gets the current wallet balance.
    /// </summary>
    /// <returns>The current balance.</returns>
    decimal GetBalance();

    /// <summary>
    /// Adds the specified amount to the wallet balance.
    /// </summary>
    /// <param name="amount">The amount to deposit (must be positive).</param>
    void Deposit(decimal amount);

    /// <summary>
    /// Deducts the specified amount from the wallet balance.
    /// </summary>
    /// <param name="amount">The amount to withdraw (must be positive and less than or equal to the current balance).</param>
    void Withdraw(decimal amount);

    /// <summary>
    /// Deducts the bet amount and adds the win amount to the wallet balance.
    /// </summary>
    /// <param name="betAmount">The bet amount (must be positive and within valid game limits).</param>
    /// <param name="winAmount">The win amount (can be zero or greater).</param>
    void PlaceBet(decimal betAmount, decimal winAmount);
}