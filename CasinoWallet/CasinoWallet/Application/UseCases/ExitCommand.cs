namespace CasinoWallet.Application.UseCases;

/// <summary>
/// Handles the logic for exiting the game loop.
/// </summary>
public class ExitCommand
{
    /// <summary>
    /// Indicates whether the game loop should continue.
    /// </summary>
    public bool ShouldExit { get; private set; }

    /// <summary>
    /// Executes the exit command by setting the exit flag.
    /// </summary>
    public void Execute()
    {
        ShouldExit = true;
    }
}