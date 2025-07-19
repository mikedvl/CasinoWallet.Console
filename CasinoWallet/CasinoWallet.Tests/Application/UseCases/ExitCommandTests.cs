using CasinoWallet.Application.UseCases;

namespace CasinoWallet.Tests.Application.UseCases;

public class ExitCommandTests
{
    [Fact]
    public void ShouldExit_DefaultValue_ShouldBeFalse()
    {
        // Arrange
        var command = new ExitCommand();

        // Assert
        Assert.False(command.ShouldExit);
    }

    [Fact]
    public void Execute_ShouldSetShouldExitToTrue()
    {
        // Arrange
        var command = new ExitCommand();

        // Act
        command.Execute();

        // Assert
        Assert.True(command.ShouldExit);
    }
}