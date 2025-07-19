using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Application.UseCases;

/// <summary>
/// Handles the logic for depositing funds into the wallet.
/// </summary>
public class DepositCommand(IWalletService walletService)
{
    /// <summary>
    /// Executes a deposit operation with the specified amount.
    /// </summary>
    /// <param name="amount">The amount to deposit (must be a positive number).</param>
    /// <returns>The updated wallet balance after deposit.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is not positive.</exception>
    public decimal Execute(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        walletService.Deposit(amount);
        return walletService.GetBalance();
    }
}