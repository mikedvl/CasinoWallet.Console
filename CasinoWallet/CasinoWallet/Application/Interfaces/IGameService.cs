namespace CasinoWallet.Application.Interfaces;

/// <summary>
/// Represents the contract for a game service that calculates winnings based on a bet.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Calculates the win amount based on the provided bet amount,
    /// according to predefined probability rules.
    /// </summary>
    /// <param name="betAmount">The bet amount placed by the player (between $1 and $10).</param>
    /// <returns>The win amount (can be zero if the bet is lost).</returns>
    decimal GenerateWin(decimal betAmount);
}