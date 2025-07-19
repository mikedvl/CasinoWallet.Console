using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.UseCases;
using Moq;

namespace CasinoWallet.Tests.Application.UseCases;

public class BetCommandTests
{
    [Fact]
    public void Execute_ValidBet_ReturnsCorrectResult()
    {
        // Arrange
        var walletMock = new Mock<IWalletService>();
        var gameMock = new Mock<IGameService>();

        decimal betAmount = 10m;
        decimal winAmount = 25m;
        decimal finalBalance = 115m;

        walletMock.Setup(w => w.GetBalance()).Returns(100m);
        gameMock.Setup(g => g.GenerateWin(betAmount)).Returns(winAmount);
        walletMock.Setup(w => w.PlaceBet(betAmount, winAmount));
        walletMock.Setup(w => w.GetBalance()).Returns(finalBalance);

        var command = new BetCommand(walletMock.Object, gameMock.Object);

        // Act
        var result = command.Execute(betAmount);

        // Assert
        Assert.Equal(betAmount, result.BetAmount);
        Assert.Equal(winAmount, result.WinAmount);
        Assert.Equal(finalBalance, result.NewBalance);

        walletMock.Verify(w => w.PlaceBet(betAmount, winAmount), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Execute_InvalidBet_ThrowsArgumentOutOfRangeException(decimal invalidAmount)
    {
        // Arrange
        var walletMock = new Mock<IWalletService>();
        var gameMock = new Mock<IGameService>();
        var command = new BetCommand(walletMock.Object, gameMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Execute(invalidAmount));
    }

    [Fact]
    public void Execute_WhenWalletThrowsInvalidOperation_ShouldPropagateException()
    {
        // Arrange
        var walletMock = new Mock<IWalletService>();
        var gameMock = new Mock<IGameService>();

        decimal betAmount = 5m;
        decimal winAmount = 0m;

        gameMock.Setup(g => g.GenerateWin(betAmount)).Returns(winAmount);
        walletMock.Setup(w => w.PlaceBet(betAmount, winAmount))
            .Throws<InvalidOperationException>();

        var command = new BetCommand(walletMock.Object, gameMock.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => command.Execute(betAmount));
    }
}