using CasinoWallet.Application.Services;
using CasinoWallet.Tests.Infrastructure.Services.Random;

namespace CasinoWallet.Tests.Application.Services;

/// <summary>
/// Deterministic tests for GameService using a fake random provider
/// to verify specific win scenarios based on controlled random values.
/// </summary>
public class GameServiceDeterministicTests
{
    [Fact]
    public void GenerateWin_LossScenario_ShouldReturnZero()
    {
        // Arrange: First roll < 0.5 means loss
        var random = new FakeRandomProvider(0.3);
        var service = new GameService(random);

        // Act
        var result = service.GenerateWin(10m);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void GenerateWin_X2Scenario_ShouldReturnExpected()
    {
        // Arrange: First roll between 0.5 and 0.9 (x2 path), second determines multiplier
        var random = new FakeRandomProvider(0.6, 0.5); // multiplier = 1.5
        var service = new GameService(random);

        // Act
        var result = service.GenerateWin(10m);

        // Assert
        Assert.Equal(15m, result); // 10 * 1.5
    }

    [Fact]
    public void GenerateWin_X10Scenario_ShouldReturnExpected()
    {
        // Arrange: First roll > 0.9 (x10 path), second determines multiplier
        var random = new FakeRandomProvider(0.95, 0.5); // multiplier = 2.0 + 0.5 * 8 = 6.0
        var service = new GameService(random);

        // Act
        var result = service.GenerateWin(10m);

        // Assert
        Assert.Equal(60m, result); // 10 * 6.0
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void GenerateWin_InvalidBet_ShouldThrow(decimal betAmount)
    {
        var random = new FakeRandomProvider(0.3); // Value doesn't matter
        var service = new GameService(random);

        Assert.Throws<ArgumentException>(() => service.GenerateWin(betAmount));
    }
}