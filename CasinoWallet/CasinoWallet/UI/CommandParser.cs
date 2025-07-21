namespace CasinoWallet.UI;

/// <summary>
/// Parses user input from the console into a command and an optional amount.
/// Expected format: "command [amount]", e.g., "deposit 100".
/// </summary>
public class CommandParser
{
    /// <summary>
    /// Parses the input string and extracts the command and amount (if present).
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <returns>
    /// A tuple containing the command (lowercased) and the parsed amount (nullable decimal).
    /// </returns>
    public (string Command, decimal? Amount) Parse(string input)
    {
        // Return empty command if input is null or whitespace
        if (string.IsNullOrWhiteSpace(input))
            return (string.Empty, null);

        // Split input by space, remove empty entries
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLower();

        decimal? amount = null;
        
        // Try to parse the second part as a decimal amount, if provided
        if (parts.Length > 1 && decimal.TryParse(parts[1], out var parsedAmount))
        {
            amount = parsedAmount;
        }

        return (command, amount);
    }
}