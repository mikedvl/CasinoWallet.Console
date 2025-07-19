using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.Models;

namespace CasinoWallet.Application.UseCases;

/// <summary>
/// Handles the logic for placing a bet and processing the result.
/// </summary>
public class BetCommand(IWalletService walletService, IGameService gameService)
{
    /// <summary>
    /// Executes a bet round using the specified amount.
    /// </summary>
    /// <param name="betAmount">The amount to bet (must be between $1 and $10).</param>
    /// <returns>A <see cref="GameResult"/> representing the outcome of the round.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if bet amount is out of the allowed range.</exception>
    public GameResult Execute(decimal betAmount)
    {
        if (betAmount is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nameof(betAmount), "Bet must be between $1 and $10.");

        var winAmount = gameService.GenerateWin(betAmount);
        walletService.PlaceBet(betAmount, winAmount);
        var newBalance = walletService.GetBalance();

        return new GameResult(betAmount, winAmount, newBalance);
    }
}