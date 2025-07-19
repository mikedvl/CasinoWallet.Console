using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Application.Services;

/// <summary>
/// Secure and certified-ready game logic implementation.
/// All win probabilities and multipliers are hardcoded and non-configurable.
/// </summary>
public class GameService(IRandomProvider randomProvider) : IGameService
{
    private readonly IRandomProvider _random = randomProvider ?? throw new ArgumentNullException(nameof(randomProvider));

    public decimal GenerateWin(decimal betAmount)
    {
        if (betAmount <= 0)
            throw new ArgumentException("Bet amount must be positive.", nameof(betAmount));

        double roll = _random.NextDouble();

        switch (roll)
        {
            case < 0.5:
                return 0m; // 50% chance to lose
            case < 0.9:
            {
                double multiplier = 1.0 + _random.NextDouble(); // 40% chance, x1.0–x2.0
                return Math.Round(betAmount * (decimal)multiplier, 2);
            }
            default:
            {
                double multiplier = 2.0 + _random.NextDouble() * 8.0; // 10% chance, x2.0–x10.0
                return Math.Round(betAmount * (decimal)multiplier, 2);
            }
        }
    }
}