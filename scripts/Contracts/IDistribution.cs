namespace Craiel.Essentials.Contracts;

public interface IDistribution
{
    int NextInt();

    long NextLong();

    float NextFloat();

    double NextDouble();

    T Clone<T>() where T : IDistribution;
}
