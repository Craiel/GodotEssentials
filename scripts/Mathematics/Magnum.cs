namespace Craiel.Essentials.Mathematics;

using System;

public struct Magnum : IComparable<Magnum>, IEquatable<Magnum>
{
    public const double Tolerance = 1e-9;
    
    public static Magnum Zero => new(0, 0);
    public static Magnum One => new(1, 0);
    
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
        if (Math.Abs(diff) > 15)
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
}
