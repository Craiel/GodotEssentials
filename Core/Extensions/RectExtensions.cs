// ReSharper disable UnusedMember.Global
namespace Craiel.Essentials.Runtime.Extensions;

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
    
    public static Rect2 Grow(this Rect2 source, Vector2 by)
    {
        Vector2 min = source.min - by;
        Vector2 max = source.max + by;
        return MinMaxRect(min, max);
    }
    
    public static Rect2 Grow(this Rect2 source, Rect2 by)
    {
        if (by.xMin < source.xMin)
        {
            source.xMin = by.xMin;
        }

        if (by.xMax > source.xMax)
        {
            source.xMax = by.xMax;
        }

        if (by.yMin < source.yMin)
        {
            source.yMin = by.yMin;
        }

        if (by.yMax > source.yMax)
        {
            source.yMax = by.yMax;
        }
        
        return source;
    }

    public static Rect2 MinMaxRect(Vector2 min, Vector2 max)
    {
        return Rect2.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    public static Rect2 Shrink(this Rect2 source, float by)
    {
        return source.Shrink(new Vector2(by, by));
    }

    public static Rect2 Shrink(this Rect2 source, Vector2 by)
    {
        Vector2 min = source.min + by;
        Vector2 max = source.max - by;

        if (max.x < min.x)
        {
            max.x = min.x;
        }

        if (max.y < min.y)
        {
            max.y = min.y;
        }
        
        return MinMaxRect(min, max);
    }
    
    public static Rect2 Fit(this Rect2 source, float width, float height, bool shrinkOnly = true)
    {
        if (shrinkOnly && source.width <= width && source.height <= height)
        {
            return source;
        }
        
        float widthPercent = width / source.width;
        float heightPercent = height / source.height;
        float fitPercent = widthPercent < heightPercent ? widthPercent : heightPercent;
        source.width *= fitPercent;
        source.height *= fitPercent;
        
        return source;
    }
    
    public static bool Includes(this Rect2 source, Rect2 target, bool includesFully = true)
    {
        if (includesFully)
        {
            return source.xMin <= target.xMin 
                   && source.xMax > target.xMax 
                   && source.yMin <= target.yMin 
                   && source.yMax >= target.yMax;
        }
        
        return target.xMax > source.xMin 
               && target.xMin < source.xMax 
               && target.yMax > source.yMin 
               && target.yMin < source.yMax;
    }
    
    public static Rect2 ResetXY(this Rect2 source)
    {
        source.x = source.y = 0;
        return source;
    }
    
    public static Rect2 Shift(this Rect2 source, float x, float y, float width, float height)
    {
        return new Rect2(source.x + x, source.y + y, source.width + width, source.height + height);
    }
    
    public static Rect2 SetX(this Rect2 source, float value)
    {
        source.x = value;
        return source;
    }

    public static Rect2 SetY(this Rect2 source, float value)
    {
        source.y = value;
        return source;
    }
    
    public static Rect2 SetHeight(this Rect2 source, float value)
    {
        source.height = value;
        return source;
    }
    
    public static Rect2 SetWidth(this Rect2 source, float value)
    {
        source.width = value;
        return source;
    }
}