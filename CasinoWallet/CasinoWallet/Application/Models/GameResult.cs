

namespace CasinoWallet.Application.Models;

/// <summary>
/// Represents the result of a single game round,
/// including the bet amount, win amount, and resulting balance.
/// </summary>
public record GameResult(decimal BetAmount, decimal WinAmount, decimal NewBalance);