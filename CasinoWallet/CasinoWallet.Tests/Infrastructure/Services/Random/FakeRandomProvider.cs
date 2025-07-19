using CasinoWallet.Application.Interfaces;

namespace CasinoWallet.Tests.Infrastructure.Services.Random;

public class FakeRandomProvider(params double[] values) : IRandomProvider
{
    private readonly Queue<double> _values = new(values);

    public double NextDouble()
    {
        if (_values.Count == 0)
            throw new InvalidOperationException("No more values in FakeRandomProvider");

        return _values.Dequeue();
    }
}