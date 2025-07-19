
using Serilog;
using CasinoWallet.Application.Interfaces;
using CasinoWallet.Application.Services;
using CasinoWallet.Application.UseCases;
using CasinoWallet.Infrastructure.Services.Random;
using CasinoWallet.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CasinoWallet;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/bootstrap-log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)
            .CreateLogger();

        try
        {
           // Log.Information("Starting CasinoWallet...");

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .UseSerilog((context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureServices((services) =>
                {
                    // Infrastructure
                    services.AddSingleton<IRandomProvider, RandomProvider>();
                    services.AddSingleton<IWalletService, WalletService>();
                    services.AddSingleton<IGameService, GameService>();

                    // Application UseCases
                    services.AddTransient<DepositCommand>();
                    services.AddTransient<WithdrawCommand>();
                    services.AddTransient<BetCommand>();
                    services.AddTransient<ExitCommand>();
                    
                    // UI
                    services.AddTransient<CommandParser>();
                    services.AddTransient<GameLoop>();
                });

            var host = builder.Build();

            var game = host.Services.GetRequiredService<GameLoop>(); 
            game.Run(); 
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}