using System;
using System.Collections.Generic;
using Craiel.Essentials.Extensions;
using Craiel.Essentials.Utils;
using Godot;

namespace Craiel.Essentials.Nodes.Tools;

[Tool]
public partial class WangTileCreator : Node
{
    private const int WangTileCount = 4;
    private static readonly IDictionary<TileType, IList<KeyValuePair<Vector2I, byte>>> PlacementMap = new Dictionary<TileType, IList<KeyValuePair<Vector2I, byte>>>();

    private static readonly TileType[] TileOrder = {
        TileType.OuterCorner,
        TileType.EdgeConnector,
        TileType.InnerCorner,
        TileType.Border,
        TileType.OverlayFill,
        TileType.UnderlayFill
    };
    
    private static readonly IDictionary<TileType, Image> TileImageCache = new Godot.Collections.Dictionary<TileType, Image>();
    
    private Texture2D lastSourceSprite;
    private int lastSourceParameterHash;
    private Image outputImageCache;

    static WangTileCreator()
    {
        RegisterPlacement(TileType.Border, 1, 0, 0);
        RegisterPlacement(TileType.Border, 3, 0, 1);
        RegisterPlacement(TileType.Border, 1, 2, 3);
        RegisterPlacement(TileType.Border, 3, 2, 2);
        
        RegisterPlacement(TileType.InnerCorner, 2, 0, 1);
        RegisterPlacement(TileType.InnerCorner, 1, 1, 0);
        RegisterPlacement(TileType.InnerCorner, 3, 1, 2);
        RegisterPlacement(TileType.InnerCorner, 2, 2, 3);
        
        RegisterPlacement(TileType.OuterCorner, 0, 0, 0);
        RegisterPlacement(TileType.OuterCorner, 0, 2, 2);
        RegisterPlacement(TileType.OuterCorner, 3, 3, 1);
        RegisterPlacement(TileType.OuterCorner, 1, 3, 3);
        
        RegisterPlacement(TileType.EdgeConnector, 0, 1, 0);
        RegisterPlacement(TileType.EdgeConnector, 2, 3, 3);
        
        RegisterPlacement(TileType.OverlayFill, 2, 1, 0);
        
        RegisterPlacement(TileType.UnderlayFill, 0, 3, 0);
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public ushort TileSize = 64;
    [Export] public float WhiteTolerance = 0.1f;
    [Export] public Texture2D SourceSprite;
    [Export] public string SavePath;
    [Export] public Sprite2D Preview;
    [Export] public bool ForceRegen;

    public enum TileType
    {
        Border,
        InnerCorner,
        OuterCorner,
        EdgeConnector,
        OverlayFill,
        UnderlayFill
    }

    public override void _ExitTree()
    {
        this.Cleanup();
        
        base._ExitTree();
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.SourceSprite == null)
        {
            this.Cleanup();
            return;
        }

        int currentParamHash = HashCode.Combine(this.TileSize, this.WhiteTolerance, this.ForceRegen);
        if (this.lastSourceSprite != this.SourceSprite || this.lastSourceParameterHash != currentParamHash)
        {
            this.lastSourceSprite = this.SourceSprite;
            this.lastSourceParameterHash = currentParamHash;
            this.RegenerateTiles();
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void RebuildTileCache()
    {
        var sourceImage = this.SourceSprite.GetImage();
        var sourceSize = sourceImage.GetSize();
        
        foreach (TileType type in EnumDefInt<TileType>.Values)
        {
            var cacheImage = Image.CreateEmpty(TileSize, TileSize, false, Image.Format.Rgba8);
            TileImageCache.Add(type, cacheImage);
            
            int tileOffset = this.GetTileOrder(type) * TileSize;
            for (var y = 0; y < TileSize; y++)
            {
                for (var x = 0; x < TileSize; x++)
                {
                    if (tileOffset + x >= sourceSize.X || y >= sourceSize.Y)
                    {
                        throw new InvalidOperationException("Out of Bounds: " + sourceSize);
                    }
                    
                    var color = sourceImage.GetPixel(tileOffset + x, y);
                    cacheImage.SetPixel(x, y, color);
                }
            }
        }
    }

    private void RegenerateTiles()
    {
        this.Cleanup();
        
        this.outputImageCache = Image.CreateEmpty(WangTileCount * TileSize, WangTileCount * TileSize, false, Image.Format.Rgba8);
        
        this.RebuildTileCache();
        Fill(this.outputImageCache, TileType.UnderlayFill);
        GenerateBorders(this.outputImageCache);
        Fill(this.outputImageCache, TileType.OverlayFill);

        if (this.Preview != null)
        {
            this.Preview.Texture = ImageTexture.CreateFromImage(this.outputImageCache);
        }

        if (!string.IsNullOrEmpty(this.SavePath))
        {
            this.outputImageCache.SavePng(this.SavePath);
        }
    }

    private void Cleanup()
    {
        if (this.Preview != null)
        {
            this.Preview.Texture = null;
        }

        foreach (Image image in TileImageCache.Values)
        {
            image.Dispose();
        }
        
        TileImageCache.Clear();
        
        if (this.outputImageCache != null)
        {
            this.outputImageCache.Dispose();
            this.outputImageCache = null;
        }
    }

    static void RegisterPlacement(TileType type, int x, int y, byte index)
    {
        if (!PlacementMap.TryGetValue(type, out var mappings))
        {
            mappings = new List<KeyValuePair<Vector2I, byte>>();
            PlacementMap.Add(type, mappings);
        }
        
        mappings.Add(new KeyValuePair<Vector2I, byte>(new Vector2I(x, y), index));
    }

    private void Fill(Image target, TileType type)
    {
        var sourceSize = TileImageCache[type].GetSize();
        var targetSize = target.GetSize();
        
        int pixelCount = WangTileCount * TileSize;
        for (var y = 0; y < pixelCount; y++)
        {
            for (var x = 0; x < pixelCount; x++)
            {
                if (x >= targetSize.X || y >= targetSize.Y)
                {
                    throw new InvalidOperationException("Target Out of Bounds: " + sourceSize + " -> " + x + ", " + y);
                }

                var sourceX = x % TileSize;
                var sourceY = y % TileSize;
                if (sourceX >= sourceSize.X || sourceY >= sourceSize.Y)
                {
                    throw new InvalidOperationException("Source Out of Bounds: " + sourceSize + " -> " + x + ", " + y);
                }
                
                var sourceColor = TileImageCache[type].GetPixel(sourceX, sourceY);
                var targetColor = target.GetPixel(x, y);

                switch (type)
                {
                    case TileType.UnderlayFill:
                    {
                        target.SetPixel(x, y, sourceColor);
                        break;
                    }

                    case TileType.OverlayFill:
                    {
                        if (targetColor.IsNearWhite())
                        {
                            var mixedColor = targetColor.Mix(sourceColor);
                            target.SetPixel(x, y, mixedColor);
                        }
                        
                        break;
                    }
                }
            }
        }
    }

    private void GenerateBorders(Image target)
    {
        foreach (TileType type in PlacementMap.Keys)
        {
            var placings = PlacementMap[type];
            var sourceImage = TileImageCache[type];
            
            for (var i = 0; i < placings.Count; i++)
            {
                var placement = placings[i];
                var position = placement.Key;
                var rotations = placement.Value;
                var rotatedImage = Image.CreateEmpty(TileSize, TileSize, false, Image.Format.Rgba8);
                rotatedImage.CopyFrom(sourceImage);

                switch (rotations)
                {
                    case 0:
                    {
                        break;
                    }

                    case 1:
                    {
                        rotatedImage.Rotate90(ClockDirection.Clockwise);
                        break;
                    }

                    case 2:
                    {
                        rotatedImage.Rotate180();
                        break;
                    }

                    case 3:
                    {
                        rotatedImage.Rotate90(ClockDirection.Counterclockwise);
                        break;
                    }
                }

                var targetPositionX = position.X * TileSize;
                var targetPositionY = position.Y * TileSize;
                for (var y = 0; y < TileSize; y++)
                {
                    for (var x = 0; x < TileSize; x++)
                    {
                        var color = rotatedImage.GetPixel(x, y);
                        if (color.A == 0)
                        {
                            continue;
                        }

                        color = target.GetPixel(targetPositionX + x, targetPositionY + y).Mix(color);
                        target.SetPixel(targetPositionX + x, targetPositionY + y, color);
                    }
                }
            }
        }
    }
    
    private int GetTileOrder(TileType type)
    {
        for (var i = 0; i < TileOrder.Length; i++)
        {
            if (TileOrder[i] == type)
            {
                return i;
            }
        }

        return -1;
    }
}