namespace CasinoWallet.UI;

public class CommandParser
{
    public (string Command, decimal? Amount) Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (string.Empty, null);

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLower();

        decimal? amount = null;
        if (parts.Length > 1 && decimal.TryParse(parts[1], out var parsedAmount))
        {
            amount = parsedAmount;
        }

        return (command, amount);
    }
}
