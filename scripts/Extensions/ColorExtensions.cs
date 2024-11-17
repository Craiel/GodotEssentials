// ReSharper disable UnusedMember.Global
namespace Craiel.Essentials.Extensions;

using System;
using System.Globalization;
using Godot;
using Utils;

public static class ColorExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static float[] ToArray(this Color source)
    {
        return new[] {source.R, source.G, source.B, source.A };
    }

    public static Color FromArray(float[] source)
    {
        return new Color(source[0], source[1], source[2], source[3]);
    }

    public static Color FromRGB(byte r, byte g, byte b)
    {
        float maxValue = byte.MaxValue;
        return new Color(r / maxValue, g / maxValue, b / maxValue);
    }

    public static Color Brighten(this Color color, float factor)
    {
        return new Color((color.R * factor).Clamp(0, 1f),
            (color.G * factor).Clamp(0, 1f),
            (color.B * factor).Clamp(0, 1f));
    }
    
    public static Color Darken(this Color color, float by)
    {
        return new Color((color.R / by).Clamp(0, 1f), 
            (color.G / by).Clamp(0, 1f), 
            (color.B / by).Clamp(0, 1f));
    }
    
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.A = alpha;
        return color;
    }
    
    public static bool IsNearWhite(this Color color, float tolerance = 0.005f)
    {
        return Mathf.Abs(color.R - 1.0) <= tolerance 
               && Mathf.Abs(color.G - 1.0) <= tolerance 
               && Mathf.Abs(color.B - 1.0) <= tolerance 
               && Math.Abs(color.A - 1.0) < Mathf.Epsilon;
    }

    public static Color Mix(this Color baseColor, Color overlay)
    {
        float alphaOverlay = overlay.A;
        float alphaBase = baseColor.A * (1.0f - alphaOverlay);
        float finalAlpha = alphaOverlay + alphaBase;
        var finalColor = overlay * alphaOverlay + baseColor * alphaBase;

        if (finalAlpha > 0)
        {
            finalColor /= finalAlpha;
        }

        finalColor.A = finalAlpha;
        return finalColor;
    }

    // Note: this is not a proper way to de-saturate, look at HSV implementations for proper ways
    public static Color DesaturateSimple(this Color color, float by)
    {
        float l = (float)(0.3 * color.R + 0.6 * color.G + 0.1 * color.B);
        float r = color.R + by * (l - color.R);
        float g = color.G + by * (l - color.G);
        float b = color.B + by * (l - color.B);
        return new Color(r, g, b, color.A);
    }
    
    public static string ToHexString(this Color color)
    {
        return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") + color.A.ToString("X2");
    }

    public static Color ToColor(this string colorHexString)
    {
        int number = int.Parse(colorHexString, NumberStyles.HexNumber);

        switch (colorHexString.Length)
        {
            case 8:
            {
                return new Color((byte)(number >> 24 & 255), (byte)(number >> 16 & 255), (byte)(number >> 8 & 255), (byte)(number & 255));
            }

            case 6:
            {
                return new Color((byte)(number >> 16 & 255), (byte)(number >> 8 & 255), (byte)(number & 255), 255);
            }

            case 4:
            {
                return new Color((byte)((number >> 12 & 15) * 17), (byte)((number >> 8 & 15) * 17), (byte)((number >> 4 & 15) * 17), (byte)((number & 15) * 17));
            }

            case 3:
            {
                return new Color((byte)((number >> 8 & 15) * 17), (byte)((number >> 4 & 15) * 17), (byte)((number & 15) * 17), 255);
            }

            default:
            {
                throw new FormatException("Support only RRGGBBAA, RRGGBB, RGBA, RGB formats");
            }
        }
    }
}
