namespace CasinoWallet.Application.Interfaces;

/// <summary>
/// Abstraction for random number generation to enable testability.
/// </summary>
public interface IRandomProvider
{
    double NextDouble();
}