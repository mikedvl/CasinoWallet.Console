using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.Services;
using CasinoWallet.Infrastructure.Services.Random;
using CasinoWallet.Tests.Infrastructure.Services.Random;
using Xunit.Abstractions;

namespace CasinoWallet.Tests.Application.Services;

public class GameServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IGameService _gameService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameServiceTests"/> class.
    /// </summary>
    /// <param name="testOutputHelper">The xUnit output helper for logging test results.</param>
    public GameServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        // Use RandomProvider as the IRandomProvider implementation
        IRandomProvider randomProvider = new RandomProvider();
        _gameService = new GameService(randomProvider);
    }

    /// <summary>
    /// Tests that the probability distribution of GenerateWin matches the specification:
    /// 50% lose, 40% win with multiplier [1.0, 2.0), 10% win with multiplier [2.0, 10.0).
    /// </summary>
    [Fact]
    public void GenerateWin_ProbabilityDistribution_MatchesSpecification()
    {
        // Arrange
        const int trials = 1_000_000; // Large number of trials for accurate statistics
        const decimal betAmount = 100m; // Fixed bet amount for simplified analysis
        int loseCount = 0; // Losses (win = 0)
        int smallWinCount = 0; // Wins with multiplier [1.0, 2.0)
        int bigWinCount = 0; // Wins with multiplier [2.0, 10.0)
        double[] multipliers = new double[trials]; // Store multipliers for analysis

        // Act
        for (int i = 0; i < trials; i++)
        {
            decimal win = _gameService.GenerateWin(betAmount);
            multipliers[i] = (double)(win / betAmount); // Store the multiplier

            switch (win)
            {
                case 0m:
                    loseCount++;
                    break;
                case >= betAmount and < betAmount * 2m:
                    smallWinCount++;
                    break;
                case >= betAmount * 2m and <= betAmount * 10m:
                    bigWinCount++;
                    break;
            }
        }

        // Assert: Verify probability distribution
        double losePercentage = (double)loseCount / trials;
        double smallWinPercentage = (double)smallWinCount / trials;
        double bigWinPercentage = (double)bigWinCount / trials;

        // Allow ±1% error due to statistical nature
        Assert.InRange(losePercentage, 0.49, 0.51); // 50% ± 1%
        Assert.InRange(smallWinPercentage, 0.39, 0.41); // 40% ± 1%
        Assert.InRange(bigWinPercentage, 0.09, 0.11); // 10% ± 1%

        // Verify multiplier ranges
        foreach (double multiplier in multipliers)
        {
            if (multiplier > 0) // Skip losses
            {
                Assert.InRange(multiplier, 1.0, 10.0); // All multipliers must be in [1.0, 10.0]
                if (multiplier < 2.0)
                    Assert.InRange(multiplier, 1.0, 2.0); // Small wins
                else
                    Assert.InRange(multiplier, 2.0, 10.0); // Big wins
            }
        }

        // Log results for analysis
        _testOutputHelper.WriteLine($"Trials: {trials}");
        _testOutputHelper.WriteLine($"Lose (0x): {loseCount} ({losePercentage:P2})");
        _testOutputHelper.WriteLine($"Small Win (1.0x to 2.0x): {smallWinCount} ({smallWinPercentage:P2})");
        _testOutputHelper.WriteLine($"Big Win (2.0x to 10.0x): {bigWinCount} ({bigWinPercentage:P2})");

        // Analyze multiplier distribution (optional)
        var smallMultipliers = multipliers.Where(m => m is >= 1.0 and < 2.0).ToArray();
        var bigMultipliers = multipliers.Where(m => m is >= 2.0 and <= 10.0).ToArray();

        _testOutputHelper.WriteLine(
            $"Small Win Multiplier Range: [{smallMultipliers.Min():F2}, {smallMultipliers.Max():F2}]");
        _testOutputHelper.WriteLine(
            $"Big Win Multiplier Range: [{bigMultipliers.Min():F2}, {bigMultipliers.Max():F2}]");

        // Calculate expected RTP (Return to Player)
        // RTP represents the average return the player can expect from a single bet.
        // It is calculated as the average multiplier applied to the bet amount.

        // Step 1: Sum all multipliers (including 0 for losses)
        double totalReturn = multipliers.Sum(); // Total sum of all returns relative to the bet

        // Step 2: Divide by number of trials to get the average return per bet
        // For example, if the totalReturn is 1,200,000 from 1,000,000 bets,
        // the RTP is 1.2, or 120% — meaning players win 120 cents per 100 cents bet on average.
        double rtp = totalReturn / trials; // Average return per bet (RTP)

        // Step 3: Log the estimated RTP as a percentage (e.g., 120.05%)
        _testOutputHelper.WriteLine($"Estimated RTP: {rtp:P2}"); // Logs something like: Estimated RTP: 120.05%
    }

    /// <summary>
    /// Tests that GenerateWin returns the expected win amount based on specific random inputs.
    /// </summary>
    /// <param name="roll">The first random value for determining the outcome.</param>
    /// <param name="multiplierRoll">The second random value for determining the multiplier.</param>
    [Theory]
    [InlineData(0.3, 0.0)] // roll < 0.5 → lose
    [InlineData(0.6, 1.0)] // 0.5–0.9 → win x2.0 (1.0 + 1.0)
    [InlineData(0.95, 0.5)] // > 0.9 → win x6.0 (2.0 + 0.5 * 8.0)
    public void GenerateWin_ShouldReturnExpectedWin(double roll, double multiplierRoll)
    {
        // Arrange
        var random = new FakeRandomProvider(roll, multiplierRoll);
        var service = new GameService(random);
        var betAmount = 10m;

        // Act
        var win = service.GenerateWin(betAmount);

        // Assert
        switch (roll)
        {
            case < 0.5:
                Assert.Equal(0m, win);
                break;
            case < 0.9:
            {
                var expected = Math.Round(betAmount * (decimal)(1.0 + multiplierRoll), 2);
                Assert.Equal(expected, win);
                break;
            }
            default:
            {
                var expected = Math.Round(betAmount * (decimal)(2.0 + multiplierRoll * 8.0), 2);
                Assert.Equal(expected, win);
                break;
            }
        }
    }

    /// <summary>
    /// Tests that GenerateWin throws an ArgumentException for invalid bet amounts (zero or negative).
    /// </summary>
    /// <param name="bet">The invalid bet amount.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void GenerateWin_ShouldThrowOnInvalidBet(decimal bet)
    {
        // Arrange
        var service = new GameService(new FakeRandomProvider(0.5, 0.5));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.GenerateWin(bet));
    }
}