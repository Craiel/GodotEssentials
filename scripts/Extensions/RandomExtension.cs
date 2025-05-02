namespace Craiel.Essentials.Extensions;

using System;
using System.Collections.Generic;
using Godot;

public static class RandomExtension
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static long NextLong(this RandomNumberGenerator rand)
    {
        return rand.NextLong(long.MinValue, long.MaxValue);
    }

    public static long NextLong(this RandomNumberGenerator rand, long min)
    {
        return rand.NextLong(min, long.MaxValue);
    }

    public static long NextLong(this RandomNumberGenerator rand, long min, long max)
    {
#pragma warning disable CS0675
        long result = rand.RandiRange((int)min >> 32, (int)max >> 32);
        result = result << 32;
        result = result | rand.RandiRange((int)min, (int)max);
        return result;
#pragma warning restore CS0675 
    }

    public static float RangeAndSign(this RandomNumberGenerator rand, float min, float max)
    {
        var result = rand.RandfRange(min, max);
        if (rand.Randf() < 0.5f)
        {
            return -result;
        }

        return result;
    }

    public static int WeightedRandom(IList<float> weights)
    {
        // sum the weights
        
        float total = 0;
        foreach (float weight in weights)
        {
            total += weight;
        }

        // select a random value between 0 and our total
        float random = EssentialCore.Random.RandfRange(0, total);

        // loop thru our weights until we arrive at the correct one
        float current = 0;
        for (int i = 0; i < weights.Count; ++i)
        {
            current += weights[i];
            if (random <= current)
            {
                return i;
            }
        }

        // shouldn't happen.
        throw new InvalidOperationException("WeightedRandom has reached an unknown outcome");
    }

    public static T WeightedRandom<T>(IList<T> values, IList<float> weights)
    {
        if (values.Count != weights.Count)
        {
            return default;
        }

        int rollIndex = WeightedRandom(weights);
        return values[rollIndex];
    }

    public static int WeightedRandom(this RandomNumberGenerator rand, IList<float> weights)
    {
        // sum the weights
        float total = 0;
        foreach (float weight in weights)
        {
            total += weight;
        }

        // select a random value between 0 and our total
        float random = rand.RandfRange(0, total);

        // loop thru our weights until we arrive at the correct one
        float current = 0;
        for (int i = 0; i < weights.Count; ++i)
        {
            current += weights[i];
            if (random <= current)
            {
                return i;
            }
        }

        // shouldn't happen.
        throw new InvalidOperationException("WeightedRandom has reached an unknown outcome");
    }

    public static T WeightedRandom<T>(this RandomNumberGenerator rand, IList<T> values, IList<float> weights)
    {
        if (values.Count != weights.Count)
        {
            return default(T);
        }

        int rollIndex = rand.WeightedRandom(weights);
        return values[rollIndex];
    }
}
