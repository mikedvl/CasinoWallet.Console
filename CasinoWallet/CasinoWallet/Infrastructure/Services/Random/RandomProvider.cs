using System.Security.Cryptography;
using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Infrastructure.Services.Random;

/// <summary>
/// Provides cryptographically secure random numbers suitable for gaming and high-integrity applications.
/// </summary>
public class RandomProvider : IRandomProvider
{
    // Thread-safe instance of RandomNumberGenerator
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    // Fixed buffer size for double precision (used locally per call, ensuring thread safety)
    private const int BufferSize = 8;

    /// <summary>
    /// Generates a cryptographically secure random double in the range [0.0, 1.0).
    /// Uses 52 bits of entropy, matching the precision of IEEE 754 double-precision floating point.
    /// </summary>
    public double NextDouble()
    {
        Span<byte> buffer = stackalloc byte[BufferSize];
        Rng.GetBytes(buffer);
        // Use the top 52 bits to match double precision
        ulong value = BitConverter.ToUInt64(buffer) >> 12;
        return value / (double)(1UL << 52);
    }
}