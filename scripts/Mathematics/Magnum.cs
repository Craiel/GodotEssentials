namespace Craiel.Essentials.Mathematics;

using System;
using Godot.Collections;

public struct Magnum : IComparable<Magnum>, IEquatable<Magnum>
{
    public const double Tolerance = 1e-9;
    
    public static Magnum Zero => new(0, 0);
    public static Magnum One => new(1, 0);
    public static Magnum MaxValue => new(double.MaxValue, long.MaxValue);

    public static Magnum operator -(Magnum a)
    {
        return new Magnum(-a.Mantissa, a.Exponent);
    }
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public Magnum(double value)
    {
        this.Mantissa = value;
        this.Exponent = 0;
        this.Normalize();
    }

    public Magnum(long value)
    {
        this.Mantissa = value;
        this.Exponent = 0;
        this.Normalize();
    }

    public Magnum(double mantissa, long exponent)
    {
        this.Mantissa = mantissa;
        this.Exponent = exponent;
        this.Normalize();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public double Mantissa;
    public long Exponent;
    
    public void Normalize()
    {
        if (Math.Abs(this.Mantissa) < Tolerance)
        {
            this.Mantissa = 0;
            this.Exponent = 0;
            return;
        }

        if (this.Mantissa != 0 && !double.IsInfinity(this.Mantissa))
        {
            int offset = (int)Math.Floor(Math.Log10(Math.Abs(this.Mantissa)));
            if (offset != 0)
            {
                this.Mantissa /= Math.Pow(10, offset);
                this.Exponent += offset;
            }
        }
    }
    
    public static Magnum operator +(Magnum a, Magnum b)
    {
        if (a.Mantissa == 0)
        {
            return b;
        }

        if (b.Mantissa == 0)
        {
            return a;
        }

        long diff = a.Exponent - b.Exponent;
        if (Math.Abs(diff) > 15) // Precision limit check
        {
            return a.Exponent > b.Exponent ? a : b;
        }

        // Align to smaller exponent using positive powers only (avoids fractional precision loss)
        if (diff > 0)
        {
            return new Magnum(a.Mantissa * GetPowerOf10(diff) + b.Mantissa, b.Exponent);
        }

        return new Magnum(a.Mantissa + b.Mantissa * GetPowerOf10(-diff), a.Exponent);
    }

    public static Magnum operator -(Magnum a, Magnum b)
    {
        if (b.Mantissa == 0)
        {
            return a;
        }

        if (a.Mantissa == 0)
        {
            return new Magnum(-b.Mantissa, b.Exponent);
        }

        long diff = a.Exponent - b.Exponent;
        if (Math.Abs(diff) > 15)
        {
            return a.Exponent > b.Exponent ? a : new Magnum(-b.Mantissa, b.Exponent);
        }

        // Align to smaller exponent using positive powers only (avoids fractional precision loss)
        if (diff > 0)
        {
            return new Magnum(a.Mantissa * GetPowerOf10(diff) - b.Mantissa, b.Exponent);
        }

        return new Magnum(a.Mantissa - b.Mantissa * GetPowerOf10(-diff), a.Exponent);
    }

    public static Magnum operator *(Magnum a, Magnum b)
    {
        return new Magnum(a.Mantissa * b.Mantissa, a.Exponent + b.Exponent);
    }

    public static Magnum operator /(Magnum a, Magnum b)
    {
        if (b.Mantissa == 0)
        {
            throw new DivideByZeroException();
        }
        
        return new Magnum(a.Mantissa / b.Mantissa, a.Exponent - b.Exponent);
    }
    
    public static Magnum operator %(Magnum a, Magnum b)
    {
        // Modulo is tricky with floats/scientific notation, implementing a basic version
        // Equivalent to: a - (b * floor(a / b))
        if (b.Mantissa == 0)
        {
             throw new DivideByZeroException();
        }
        
        Magnum quotient = a / b;
        double floorQuotient = Math.Floor(quotient.ToDouble()); // Warning: precision loss if quotient is huge
        
        // If the quotient is too large to fit in double, modulo typically results in the original value or is undefined in this context
        // For game purposes, we likely won't mod huge numbers by small ones often, but let's handle the safe case
        if (double.IsInfinity(floorQuotient))
        {
             return Zero; 
        }

        return a - (b * new Magnum(floorQuotient));
    }

    public static bool operator ==(Magnum a, Magnum b)
    {
        return a.Exponent == b.Exponent && Math.Abs(a.Mantissa - b.Mantissa) < Tolerance;
    }

    public static bool operator !=(Magnum a, Magnum b)
    {
        return !(a == b);
    }

    public static bool operator >(Magnum a, Magnum b)
    {
        bool aIsNegative = a.Mantissa < 0;
        bool bIsNegative = b.Mantissa < 0;

        if (aIsNegative != bIsNegative)
        {
            return bIsNegative;
        }

        if (a.Exponent != b.Exponent)
        {
            return aIsNegative ? a.Exponent < b.Exponent : a.Exponent > b.Exponent;
        }

        return a.Mantissa > b.Mantissa;
    }

    public static bool operator <(Magnum a, Magnum b)
    {
        bool aIsNegative = a.Mantissa < 0;
        bool bIsNegative = b.Mantissa < 0;

        if (aIsNegative != bIsNegative)
        {
            return aIsNegative;
        }

        if (a.Exponent != b.Exponent)
        {
            return aIsNegative ? a.Exponent > b.Exponent : a.Exponent < b.Exponent;
        }

        return a.Mantissa < b.Mantissa;
    }

    public static bool operator >=(Magnum a, Magnum b)
    {
        return a > b || a == b;
    }

    public static bool operator <=(Magnum a, Magnum b)
    {
        return a < b || a == b;
    }
    
    public static implicit operator Magnum(double value)
    {
        return new Magnum(value);
    }

    public static implicit operator Magnum(long value)
    {
        return new Magnum(value);
    }

    public static implicit operator Magnum(int value)
    {
        return new Magnum(value);
    }

    public static implicit operator Magnum(float value)
    {
        return new Magnum(value);
    }

    public static explicit operator double(Magnum value)
    {
        return value.ToDouble();
    }

    public static explicit operator long(Magnum value)
    {
        return (long)value.ToDouble();
    }

    public static explicit operator int(Magnum value)
    {
        return (int)value.ToDouble();
    }

    public override bool Equals(object obj)
    {
        return obj is Magnum other && Equals(other);
    }

    public bool Equals(Magnum other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Mantissa, Exponent);
    }
    
    public int CompareTo(Magnum other)
    {
        if (this < other)
        {
            return -1;
        }

        if (this > other)
        {
            return 1;
        }

        return 0;
    }

    public override string ToString()
    {
        return Craiel.Essentials.Formatting.FormattingExtensions.Format(this, 2);
    }
    
    public double ToDouble()
    {
        return this.Mantissa * Math.Pow(10, this.Exponent);
    }

    public string ToString(string format)
    {
        // For now ignore format or use basic implementation
        return this.ToString();
    }

    public Magnum Floor()
    {
        if (this.Exponent >= 0)
        {
             // If exponent is positive, it's already an integer effectively for large numbers, 
             // or check if mantissa has decimals? 
             // For large exponent, decimals in mantissa represent integers. 
             // Simplified: ToDouble floor if small, else return this.
             if (this.Exponent > 15) return this;
             return new Magnum(Math.Floor(this.ToDouble()));
        }
        return new Magnum(Math.Floor(this.ToDouble()));
    }

    public Magnum Ceiling()
    {
        if (this.Exponent > 15) return this;
        return new Magnum(Math.Ceiling(this.ToDouble()));
    }

    public static Magnum Min(Magnum a, Magnum b)
    {
        return a < b ? a : b;
    }

    public static Magnum Max(Magnum a, Magnum b)
    {
        return a > b ? a : b;
    }

    public static Magnum Clamp(Magnum value, Magnum min, Magnum max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static Magnum Abs(Magnum value)
    {
        return new Magnum(Math.Abs(value.Mantissa), value.Exponent);
    }
    
    public Magnum Round()
    {
        if (this.Exponent > 15) return this;
        return new Magnum(Math.Round(this.ToDouble()));
    }

    public void SaveTo(Dictionary target, string key)
    {
        target[key + "_m"] = this.Mantissa;
        target[key + "_e"] = this.Exponent;
    }

    public static Magnum LoadFrom(Dictionary source, string key)
    {
        double mantissa = source[key + "_m"].AsDouble();
        long exponent = source[key + "_e"].AsInt64();
        return new Magnum(mantissa, exponent);
    }

    private static double GetPowerOf10(long exponent)
    {
        return exponent switch
        {
            0 => 1.0,
            1 => 10.0,
            2 => 100.0,
            3 => 1000.0,
            4 => 10000.0,
            5 => 100000.0,
            6 => 1000000.0,
            7 => 10000000.0,
            8 => 100000000.0,
            9 => 1000000000.0,
            10 => 10000000000.0,
            11 => 100000000000.0,
            12 => 1000000000000.0,
            13 => 10000000000000.0,
            14 => 100000000000000.0,
            15 => 1000000000000000.0,
            _ => Math.Pow(10, exponent)
        };
    }
}