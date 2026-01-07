namespace Craiel.Essentials.Mathematics;

using System;
using Godot.Collections;

public struct Magnum : IComparable<Magnum>, IEquatable<Magnum>
{
    /// <summary>
    /// Maximum significant digits in double-precision floating point.
    /// Beyond this exponent difference, smaller values are lost to precision.
    /// </summary>
    const int DoublePrecisionDigits = 15;

    /// <summary>
    /// Maximum exponent that can be represented by a double (~1.8e308).
    /// </summary>
    const int DoubleMaxExponent = 308;

    /// <summary>
    /// Maximum exponent that can be represented by a float (~3.4e38).
    /// </summary>
    const int FloatMaxExponent = 38;

    public const double Tolerance = 1e-9;

    public static Magnum Zero => new(0, 0);
    public static Magnum One => new(1, 0);
    public static Magnum MaxValue => new(1, 308);

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
        if (Math.Abs(diff) > DoublePrecisionDigits)
        {
            return a.Exponent > b.Exponent ? a : b;
        }

        return new Magnum(a.Mantissa + b.Mantissa * Math.Pow(10, -diff), a.Exponent);
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
        if (Math.Abs(diff) > DoublePrecisionDigits)
        {
            return a.Exponent > b.Exponent ? a : new Magnum(-b.Mantissa, b.Exponent);
        }

        return new Magnum(a.Mantissa - b.Mantissa * Math.Pow(10, -diff), a.Exponent);
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
        if (a.Exponent != b.Exponent)
        {
            return a.Exponent > b.Exponent;
        }
        return a.Mantissa > b.Mantissa;
    }

    public static bool operator <(Magnum a, Magnum b)
    {
        if (a.Exponent != b.Exponent)
        {
            return a.Exponent < b.Exponent;
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
#if DEBUG
        if (this.Exponent > DoubleMaxExponent)
        {
            throw new OverflowException($"Magnum value {this} exceeds double.MaxValue (exponent {this.Exponent} > {DoubleMaxExponent})");
        }
#endif
        
        return this.Mantissa * Math.Pow(10, this.Exponent);
    }

    public float ToFloat()
    {
#if DEBUG
        if (this.Exponent > FloatMaxExponent)
        {
            throw new OverflowException($"Magnum value {this} exceeds float.MaxValue (exponent {this.Exponent} > {FloatMaxExponent})");
        }
#endif
        
        return (float)this.ToDouble();
    }

    public string ToString(string format)
    {
        // For now ignore format or use basic implementation
        return this.ToString();
    }

    public Magnum Floor()
    {
        if (this.Exponent > DoublePrecisionDigits)
        {
            return this;
        }

        return new Magnum(Math.Floor(this.ToDouble()));
    }

    public Magnum Ceiling()
    {
        if (this.Exponent > DoublePrecisionDigits)
        {
            return this;
        }

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
        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    public static Magnum Abs(Magnum value)
    {
        return new Magnum(Math.Abs(value.Mantissa), value.Exponent);
    }
    
    public Magnum Round()
    {
        if (this.Exponent > DoublePrecisionDigits)
        {
            return this;
        }

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
}