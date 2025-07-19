using CasinoWallet.Application.Interfaces;
using CasinoWallet.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CasinoWallet.Application.Services;

/// <summary>
/// Provides wallet-related operations including deposits, withdrawals, and balance updates after bets.
/// </summary>
public class WalletService(ILogger<WalletService> logger) : IWalletService
{
    private readonly Wallet _wallet = new();
    private readonly ILogger<WalletService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Returns the current wallet balance.
    /// </summary>
    public decimal GetBalance()
    {
        var balance = _wallet.Balance;
        _logger.LogInformation("Retrieved wallet balance: {Balance}", balance);
        return balance;
    }

    /// <summary>
    /// Adds the specified amount to the wallet balance.
    /// </summary>
    public void Deposit(decimal amount)
    {
        _logger.LogInformation("Attempting to deposit: {Amount}", amount);
        _wallet.Deposit(amount);
        _logger.LogInformation("Deposit successful. New balance: {Balance}", _wallet.Balance);
    }

    /// <summary>
    /// Withdraws the specified amount from the wallet balance.
    /// </summary>
    public void Withdraw(decimal amount)
    {
        _logger.LogInformation("Attempting to withdraw: {Amount}", amount);
        _wallet.Withdraw(amount);
        _logger.LogInformation("Withdrawal successful. New balance: {Balance}", _wallet.Balance);
    }

    /// <summary>
    /// Processes a bet by subtracting the bet amount and applying the win amount to the wallet balance.
    /// </summary>
    public void PlaceBet(decimal betAmount, decimal winAmount)
    {
        _logger.LogInformation("Placing bet. Bet amount: {BetAmount}, Win amount: {WinAmount}", betAmount, winAmount);
        _wallet.ApplyBet(betAmount);
        _wallet.ApplyWin(winAmount);
        _logger.LogInformation("Bet processed. New balance: {Balance}", _wallet.Balance);
    }
}