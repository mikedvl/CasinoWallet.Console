using CasinoWallet.Application.UseCases;
using Microsoft.Extensions.Logging;

namespace CasinoWallet.UI;

/// <summary>
/// Handles user interaction via the console and coordinates application use cases.
/// </summary>
public class GameLoop(
    DepositCommand depositCommand,
    WithdrawCommand withdrawCommand,
    BetCommand betCommand,
    ExitCommand exitCommand,
    CommandParser parser,
    ILogger<GameLoop> logger)
{
    public void Run()
    {
        Console.WriteLine("Welcome to the Casino!");
        Console.WriteLine("Please, submit action:");
        logger.LogInformation("Start");

        
        while (!exitCommand.ShouldExit)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            var (command, amount) = parser.Parse(input);

            try
            {
                logger.LogInformation("Received command: {Command} {Amount}", command, amount);
                
                switch (command)
                {
                    case "deposit":
                        if (amount is not > 0)
                        {
                            logger.LogWarning("Invalid input: {Input}", input);
                            Console.WriteLine("Invalid deposit amount.");
                            break;
                        }

                        var afterDeposit = depositCommand.Execute(amount.Value);
                        Console.WriteLine(
                            $"Your deposit of ${amount:F2} was successful. Your current balance is: ${afterDeposit:F2}");
                        break;

                    case "withdraw":
                        if (amount is not > 0)
                        {
                            Console.WriteLine("Invalid withdrawal amount.");
                            break;
                        }

                        var afterWithdraw = withdrawCommand.Execute(amount.Value);
                        Console.WriteLine(
                            $"Your withdrawal of ${amount:F2} was successful. Your current balance is: ${afterWithdraw:F2}");
                        break;

                    case "bet":
                        if (amount is not > 0)
                        {
                            Console.WriteLine("Invalid bet amount.");
                            break;
                        }

                        var result = betCommand.Execute(amount.Value);
                        
                        logger.LogInformation("Bet executed: BetAmount={BetAmount}, WinAmount={WinAmount}, NewBalance={NewBalance}",
                            result.BetAmount, result.WinAmount, result.NewBalance);
                        
                        Console.WriteLine(result.WinAmount == 0
                            ? $"No luck this time! Your current balance is: ${result.NewBalance:F2}"
                            : $"Congrats - you won ${result.WinAmount:F2}! Your current balance is: ${result.NewBalance:F2}");
                        break;

                    case "exit":
                        exitCommand.Execute();
                        Console.WriteLine("Thank you for playing! Hope to see you again soon.\n");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("Invalid command. Try: 'deposit 10', 'withdraw 5', 'bet 1', or 'exit'.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                logger.LogError(ex, "Exception during command execution: {Input}", input);
            }
        }
    }
}