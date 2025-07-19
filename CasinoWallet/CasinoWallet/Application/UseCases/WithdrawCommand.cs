using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Application.UseCases;

/// <summary>
/// Handles the logic for withdrawing funds from the wallet.
/// </summary>
public class WithdrawCommand(IWalletService walletService)
{
    /// <summary>
    /// Executes a withdrawal operation with the specified amount.
    /// </summary>
    /// <param name="amount">The amount to withdraw (must be a positive number and not exceed the current balance).</param>
    /// <returns>The updated wallet balance after withdrawal.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the balance is insufficient.</exception>
    public decimal Execute(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        walletService.Withdraw(amount);
        return walletService.GetBalance();
    }
}