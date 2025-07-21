using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Application.Services;

/// <summary>
/// Secure and certified-ready game logic implementation.
/// All win probabilities and multipliers are hardcoded and non-configurable.
/// </summary>
public class GameService(IRandomProvider randomProvider) : IGameService
{
    private readonly IRandomProvider _random = randomProvider ?? throw new ArgumentNullException(nameof(randomProvider));

    // Win probability thresholds (must sum up to 1.0 or less)
    private const double LoseThreshold = 0.5;      // 50% lose
    private const double SmallWinThreshold = 0.9;    // 40% small win
    // Remaining 10% is big win

    // Multiplier ranges
    private const double SmallWinMinMultiplier = 1.0;
    private const double SmallWinMaxMultiplier = 2.0;

    private const double BigWinMinMultiplier = 2.0;
    private const double BigWinMaxMultiplier = 10.0;

    public decimal GenerateWin(decimal betAmount)
    {
        if (betAmount <= 0)
            throw new ArgumentException("Bet amount must be positive.", nameof(betAmount));

        double roll = _random.NextDouble();

        switch (roll)
        {
            case < LoseThreshold:
                return 0m; // Lose
            case < SmallWinThreshold:
            {
                // Small win: multiplier between 1.0 and 2.0
                double multiplier = SmallWinMinMultiplier + _random.NextDouble() * (SmallWinMaxMultiplier - SmallWinMinMultiplier);
                return Math.Round(betAmount * (decimal)multiplier, 2);
            }
            default:
            {
                // Big win: multiplier between 2.0 and 10.0
                double bigMultiplier = BigWinMinMultiplier + _random.NextDouble() * (BigWinMaxMultiplier - BigWinMinMultiplier);
                return Math.Round(betAmount * (decimal)bigMultiplier, 2);
            }
        }
    }
}