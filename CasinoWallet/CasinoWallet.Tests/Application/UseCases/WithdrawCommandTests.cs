using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.UseCases;
using Moq;

namespace CasinoWallet.Tests.Application.UseCases;

public class WithdrawCommandTests
{
    private readonly Mock<IWalletService> _walletServiceMock;
    private readonly WithdrawCommand _command;

    public WithdrawCommandTests()
    {
        _walletServiceMock = new Mock<IWalletService>();
        _command = new WithdrawCommand(_walletServiceMock.Object);
    }

    [Fact]
    public void Execute_ValidAmount_ShouldWithdrawAndReturnBalance()
    {
        // Arrange
        decimal amount = 50m;
        decimal expectedBalance = 150m;

        _walletServiceMock.Setup(ws => ws.Withdraw(amount));
        _walletServiceMock.Setup(ws => ws.GetBalance()).Returns(expectedBalance);

        // Act
        var result = _command.Execute(amount);

        // Assert
        _walletServiceMock.Verify(ws => ws.Withdraw(amount), Times.Once);
        _walletServiceMock.Verify(ws => ws.GetBalance(), Times.Once);
        Assert.Equal(expectedBalance, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Execute_InvalidAmount_ShouldThrowArgumentException(decimal amount)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _command.Execute(amount));
        Assert.Equal("Withdrawal amount must be positive. (Parameter 'amount')", ex.Message);
    }

    [Fact]
    public void Execute_InsufficientFunds_ShouldThrowInvalidOperationException()
    {
        // Arrange
        decimal amount = 100m;
        _walletServiceMock.Setup(ws => ws.Withdraw(amount)).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _command.Execute(amount));

        _walletServiceMock.Verify(ws => ws.Withdraw(amount), Times.Once);
        _walletServiceMock.Verify(ws => ws.GetBalance(), Times.Never); // Balance should not be checked if withdraw fails
    }
}