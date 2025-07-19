using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CasinoWallet.Tests.Domain.Entities;

public class WalletTests
{
    private IWalletService CreateWalletService()
    {
        var loggerMock = new Mock<ILogger<WalletService>>();
        return new WalletService(loggerMock.Object);
    }

    [Fact]
    public void InitialBalance_ShouldBeZero()
    {
        var wallet = CreateWalletService();
        var balance = wallet.GetBalance();
        Assert.Equal(0m, balance);
    }

    [Fact]
    public void Deposit_ShouldIncreaseBalance()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        var balance = wallet.GetBalance();
        Assert.Equal(100m, balance);
    }

    [Fact]
    public void Withdraw_ShouldDecreaseBalance()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        wallet.Withdraw(40m);
        var balance = wallet.GetBalance();
        Assert.Equal(60m, balance);
    }

    [Fact]
    public void Withdraw_MoreThanBalance_ShouldThrow()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(50m);
        Assert.Throws<InvalidOperationException>(() => wallet.Withdraw(60m));
    }

    [Fact]
    public void PlaceBet_ShouldUpdateBalanceCorrectly()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        wallet.PlaceBet(20m, 50m);
        var balance = wallet.GetBalance();
        Assert.Equal(130m, balance); // 100 - 20 + 50
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public void Deposit_InvalidAmount_ShouldThrow(decimal amount)
    {
        var wallet = CreateWalletService();
        Assert.Throws<ArgumentException>(() => wallet.Deposit(amount));
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public void Withdraw_InvalidAmount_ShouldThrow(decimal amount)
    {
        var wallet = CreateWalletService();
        Assert.Throws<ArgumentException>(() => wallet.Withdraw(amount));
    }

    [Fact]
    public void PlaceBet_WithNegativeBet_ShouldThrow()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(50m);
        Assert.Throws<ArgumentException>(() => wallet.PlaceBet(-5m, 10m));
    }

    [Fact]
    public void PlaceBet_WithZeroBet_ShouldThrow()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(50m);
        Assert.Throws<ArgumentException>(() => wallet.PlaceBet(0m, 10m));
    }

    [Fact]
    public void PlaceBet_WithBetGreaterThanBalance_ShouldThrow()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(10m);
        Assert.Throws<InvalidOperationException>(() => wallet.PlaceBet(20m, 10m));
    }

    [Fact]
    public void PlaceBet_WithZeroWin_ShouldOnlySubtractBet()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        wallet.PlaceBet(10m, 0m);
        var balance = wallet.GetBalance();
        Assert.Equal(90m, balance);
    }

    [Fact]
    public void PlaceBet_WithNegativeWin_ShouldThrow()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        Assert.Throws<ArgumentException>(() => wallet.PlaceBet(10m, -5m));
    }

    [Fact]
    public void MultipleOperations_ShouldResultInExpectedBalance()
    {
        var wallet = CreateWalletService();
        wallet.Deposit(100m);
        wallet.Withdraw(30m);
        wallet.PlaceBet(20m, 40m);
        var balance = wallet.GetBalance(); // 100 - 30 - 20 + 40 = 90
        Assert.Equal(90m, balance);
    }
}