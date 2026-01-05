namespace Craiel.Essentials.Formatting;

using System;
using System.Globalization;
using Enums;
using Godot;
using Mathematics;

public static class FormattingExtensions
{
    // ReSharper disable StringLiteralTypo
    private static readonly NumberDefinition[] NumberDefinitions =
    {
        new("Million", "M", 6),
        new("Billion", "B", 9),
        new("Trillion", "T", 12),
        new("Quadrillion", "Qa", 15),
        new("Quintillion", "Qi", 18),
        new("Sextillion", "Sx", 21),
        new("Septillion", "Sp", 24),
        new("Octillion", "Oc", 27),
        new("Nonillion", "No", 30),
        new("Decillion", "Dc", 33),
        new("Undecillion", "Ud", 36),
        new("Duodecillion", "Dd", 39),
        new("Tredecillion", "Td", 42),
        new("Quattuordecillion", "Qad", 45),
        new("Quinquadecillion", "Qid", 48),
        new("Sedecillion", "Sxd", 51),
        new("Septendecillion", "Spd", 54),
        new("Octodecillion", "Ocd", 57),
        new("Novendecillion", "Nod", 60),
        new("Vigintillion", "Vg", 63),
        new("Unvigintillion", "Uvg", 66),
        new("Duovigintillion", "Dvg", 69),
        new("Tresvigintillion", "Tvg", 72),
        new("Quattuorvigintillion", "Qavg", 75),
        new("Quinquavigintillion", "Qivg", 78),
        new("Sesvigintillion", "Sxvg", 81),
        new("Septemvigintillion", "Spvg", 84),
        new("Octovigintillion", "Ocvg", 87),
        new("Novemvigintillion", "Novg", 90),
        new("Trigintillion", "Tg", 93),
        new("Untrigintillion", "Utg", 96),
        new("Duotrigintillion", "Dtg", 99),
        new("Trestrigintillion", "Ttg", 102),
        new("Quattuortrigintillion", "Qatg", 105),
        new("Quinquatrigintillion", "Qitg", 108),
        new("Sestrigintillion", "Sxtg", 111),
        new("Septentrigintillion", "Sptg", 114),
        new("Octotrigintillion", "Octg", 117),
        new("Noventrigintillion", "Notg", 120),
        new("Quadragintillion", "G", 123),
        new("Unquadragintillion", "Ug", 126),
        new("Duoquadragintillion", "Dg", 129),
        new("Tresquadragintillion", "Tg", 132),
        new("Quattorquadragintillion", "Qag", 135),
        new("Quinquaquadragintillion", "Qig", 138),
        new("Quinquaquadragintillion", "Qig", 141)
    };
    // ReSharper enable StringLiteralTypo
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static string DefaultFormatPrefix;
    
    public static string Format(this ulong value, byte decimalDigits = 0, NumberFormattingType type = NumberFormattingType.ShortName, string customPrefix = null)
    {
        return ((double) value).Format(decimalDigits, type);
    }
    
    public static string Format(this float value, byte decimalDigits = 0, NumberFormattingType type = NumberFormattingType.ShortName, string customPrefix = null)
    {
        return ((double) value).Format(decimalDigits, type);
    }
    
    public static string Format(this double value, byte decimalDigits = 0, NumberFormattingType type = NumberFormattingType.ShortName, string customPrefix = null)
    {
        string prefix = customPrefix ?? DefaultFormatPrefix ?? string.Empty;
        
        NumberDefinition? definition = null;
        if (type != NumberFormattingType.Raw)
        {
            definition = GetNumberDefinition(value);
        }

        if (decimalDigits != byte.MaxValue)
        {
            value = Math.Round(value, decimalDigits);
        }

        if (value < NumberDefinitions[0].Value)
        {
            return $"{prefix}{value:#,##0.##}";
        }

        if (definition != null)
        {
            value /= definition.Value.Value;
            if (value >= 100)
            {
                value = Math.Round(value, 1);
            }
            else if (value >= 10)
            {
                value = Math.Round(value, 2);
            }
            else
            {
                value = Math.Round(value, 3);
            }
        }

        if (definition == null)
        {
            return $"{prefix}{value}";
        }

        switch (type)
        {
            case NumberFormattingType.ShortName:
            {
                return $"{prefix}{value} {definition.Value.ShortName}";
            }

            case NumberFormattingType.FullName:
            {
                return $"{prefix}{value} {definition.Value.FullName}";
            }

            default:
            {
                return prefix + value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
    
    public static string Format(this Magnum value, byte decimalDigits = 0, NumberFormattingType type = NumberFormattingType.ShortName, string customPrefix = null)
    {
        string prefix = customPrefix ?? DefaultFormatPrefix ?? string.Empty;
        
        NumberDefinition? definition = null;
        if (type != NumberFormattingType.Raw)
        {
            definition = GetNumberDefinition(value);
        }

        double displayValue = value.Mantissa;
        
        if (definition != null)
        {
            double definitionExponent = Math.Log10(definition.Value.Value);
            double exponentDiff = value.Exponent - definitionExponent;
            displayValue = value.Mantissa * Math.Pow(10, exponentDiff);
            
            if (displayValue >= 100)
            {
                displayValue = Math.Round(displayValue, 1);
            }
            else if (displayValue >= 10)
            {
                displayValue = Math.Round(displayValue, 2);
            }
            else
            {
                displayValue = Math.Round(displayValue, 3);
            }
        }
        else
        {
             if (value.Exponent < 6)
             {
                 return value.ToDouble().Format(decimalDigits, type, customPrefix);
             }
        }

        if (definition == null)
        {
            return $"{prefix}{value}";
        }

        switch (type)
        {
            case NumberFormattingType.ShortName:
            {
                return $"{prefix}{displayValue} {definition.Value.ShortName}";
            }

            case NumberFormattingType.FullName:
            {
                return $"{prefix}{displayValue} {definition.Value.FullName}";
            }

            default:
            {
                return prefix + displayValue.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static NumberDefinition? GetNumberDefinition(Magnum value)
    {
        long exponents = value.Exponent;
        
        if (exponents >= 6)
        {
            long formatIndex = (long)Mathf.Floor((exponents - 6) / 3f);
            if (formatIndex >= NumberDefinitions.Length)
            {
                return NumberDefinitions[^1];
            }
            
            if (formatIndex < 0)
            {
                return null;
            }

            return NumberDefinitions[formatIndex];
        }

        return null;
    }

    private static NumberDefinition? GetNumberDefinition(double value)
    {
        uint exponents = (uint)Math.Floor(Math.Log10(value));
        if (exponents >= 6)
        {
            uint formatIndex = (uint)Mathf.Floor((exponents - 6) / 3f);
            if (formatIndex >= NumberDefinitions.Length)
            {
                return NumberDefinitions[^1];
            }
            
            return NumberDefinitions[formatIndex];
        }

        return null;
    }
    
    private struct NumberDefinition(string name, string shortName, ushort exponent)
    {
        public readonly string FullName = name;
        public readonly string ShortName = shortName;
        public readonly double Value = Math.Pow(10, exponent);
    }
}
