namespace Craiel.Essentials.Utils;

using System.Collections.Generic;
using Godot;

public static class DisplayUtils
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static Vector2I[] AllWindowSizes = {
        new(800, 600),
        new(1024, 768),
        new(1280, 720),
        new(1280, 768),
        new(1280, 800),
        new(1280, 1024),
        new(1360, 768),
        new(1366, 768),
        new(1440, 900),
        new(1440, 1080),
        new(1600, 900),
        new(1680, 1050),
        new(1920, 1080),
        new(2560, 1080),
        new(2560, 1440),
        new(3840, 1080),
        new(3840, 2160),
    };
    
    public static readonly IList<Vector2I> SupportedWindowSizes = new List<Vector2I>();
    
    public static void RefreshWindowSizes(int screen)
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize(screen);
        
        SupportedWindowSizes.Clear();
        for (var i = 0; i < AllWindowSizes.Length; i++)
        {
            var entry = AllWindowSizes[i];
            if (entry.X > screenSize.X || entry.Y > screenSize.Y)
            {
                continue;
            }
            
            SupportedWindowSizes.Add(entry);
        }
    }
}