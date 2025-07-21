using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.Services;
using CasinoWallet.Application.UseCases;
using CasinoWallet.Infrastructure.Services.Random;
using CasinoWallet.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CasinoWallet.Tests.Smoke;

public class SmokeTests
{
    [Fact]
    public void CasinoWallet_Startup_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IRandomProvider, RandomProvider>();
                    services.AddSingleton<IWalletService, WalletService>();
                    services.AddSingleton<IGameService, GameService>();
                    services.AddTransient<DepositCommand>();
                    services.AddTransient<WithdrawCommand>();
                    services.AddTransient<BetCommand>();
                    services.AddTransient<ExitCommand>();
                    services.AddTransient<CommandParser>();
                    services.AddTransient<GameLoop>();
                });

            using var host = builder.Build();
            host.Services.GetRequiredService<GameLoop>();
        });

        Assert.Null(exception);  
    }
}