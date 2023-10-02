// ReSharper disable UnusedMember.Global
namespace Craiel.Essentials.Extensions;

using Godot;

public static class RectExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static Rect2 Grow(this Rect2 source, float by)
    {
        return source.Grow(new Vector2(by, by));
    }
    
    public static Rect2 Grow(this Rect2 source, Vector2 by, bool keepPosition = false)
    {
        if (keepPosition)
        {
            return new Rect2(source.Position, source.Size + by);
        }
        
        return new Rect2(source.Position - (by / 2f), source.Size + by);
    }
    
    public static Rect2 MinMaxRect(Vector2 min, Vector2 max)
    {
        return new Rect2(min, max - min);
    }

    public static Rect2 Shrink(this Rect2 source, float by)
    {
        return source.Shrink(new Vector2(by, by));
    }

    public static Rect2 Shrink(this Rect2 source, Vector2 by, bool keepPosition = false)
    {
        if (keepPosition)
        {
            return new Rect2(source.Position, source.Size - by);
        }

        return new Rect2(source.Position + (by / 2f), source.Size - by);
    }
    
    public static Rect2 Fit(this Rect2 source, float width, float height, bool shrinkOnly = true)
    {
        if (shrinkOnly && source.Size.X <= width && source.Size.Y <= height)
        {
            return source;
        }
        
        float widthPercent = width / source.Size.X;
        float heightPercent = height / source.Size.Y;
        float fitPercent = widthPercent < heightPercent ? widthPercent : heightPercent;
        Vector2 newSize = new Vector2(source.Size.X * fitPercent, source.Size.Y * fitPercent);
        return new Rect2(source.Position, newSize);
    }
    
    public static bool Overlaps(this Rect2 source, Rect2 target)
    {
        return target.End.X > source.Position.X 
               && target.Position.X < source.End.X 
               && target.End.Y > source.Position.Y 
               && target.Position.Y < source.End.Y;
    }
    
    public static Rect2 ResetXY(this Rect2 source)
    {
        return new Rect2(Vector2.Zero, source.Size);
    }
    
    public static Rect2 Shift(this Rect2 source, float x, float y, float width, float height)
    {
        return source.Shift(new Vector2(x, y), new Vector2(width, height));
    }
    
    public static Rect2 Shift(this Rect2 source, Vector2 xy, Vector2 widthHeight)
    {
        return new Rect2(source.Position + xy, source.Size + widthHeight);
    }
    
    public static Rect2 SetX(this Rect2 source, float value)
    {
        return new Rect2(value, source.Position.Y, source.Size);
    }

    public static Rect2 SetY(this Rect2 source, float value)
    {
        return new Rect2(source.Position.X, 0, source.Size);
    }
    
    public static Rect2 SetHeight(this Rect2 source, float value)
    {
        return new Rect2(source.Position, source.Size.X, value);
    }
    
    public static Rect2 SetWidth(this Rect2 source, float value)
    {
        return new Rect2(source.Position, value, source.Size.Y);
    }
}