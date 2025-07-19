using Moq;

namespace CasinoWallet.Tests.Application.UseCases;
using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.UseCases;

public class DepositCommandTests
{
    [Fact]
    public void Execute_ShouldIncreaseBalance_WhenAmountIsValid()
    {
        // Arrange
        var walletMock = new Mock<IWalletService>();
        walletMock.Setup(w => w.Deposit(100m));
        walletMock.Setup(w => w.GetBalance()).Returns(200m);

        var command = new DepositCommand(walletMock.Object);

        // Act
        var result = command.Execute(100m);

        // Assert
        Assert.Equal(200m, result);
        walletMock.Verify(w => w.Deposit(100m), Times.Once);
        walletMock.Verify(w => w.GetBalance(), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Execute_ShouldThrow_WhenAmountIsInvalid(decimal invalidAmount)
    {
        // Arrange
        var walletMock = new Mock<IWalletService>();
        var command = new DepositCommand(walletMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => command.Execute(invalidAmount));
        walletMock.Verify(w => w.Deposit(It.IsAny<decimal>()), Times.Never);
    }
}