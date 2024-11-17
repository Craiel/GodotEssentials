namespace Craiel.Essentials.Nodes;

using System;
using System.Collections.Generic;
using Collections;
using Godot;

[Tool]
public partial class DualTileMapLayer : TileMapLayer
{
    private const string TileMapDisplayNodeName = "WorldTileMap";
    private static readonly Vector2I FullTile = new(2, 1);
    private static readonly Vector2I EmptyTile = new(0, 3);
    
    private static readonly IDictionary<Direction, TileSet.CellNeighbor> NeighborMap = new Dictionary<Direction, TileSet.CellNeighbor>()
        {
            { Direction.Top , TileSet.CellNeighbor.TopSide },
            { Direction.Left , TileSet.CellNeighbor.LeftSide },
            { Direction.Right , TileSet.CellNeighbor.RightSide },
            { Direction.Bottom , TileSet.CellNeighbor.BottomSide },
            { Direction.TopLeft , TileSet.CellNeighbor.TopLeftCorner },
            { Direction.TopRight , TileSet.CellNeighbor.TopRightCorner },
            { Direction.BottomLeft , TileSet.CellNeighbor.BottomLeftCorner },
            { Direction.BottomRight , TileSet.CellNeighbor.BottomRightCorner }
        };
    
    private static readonly IDictionary<Direction, TileSet.CellNeighbor> NeighborMapIsometric = new Dictionary<Direction, TileSet.CellNeighbor>
    {
        { Direction.Top , TileSet.CellNeighbor.TopRightSide },
        { Direction.Left , TileSet.CellNeighbor.TopLeftSide },
        { Direction.Right , TileSet.CellNeighbor.BottomRightSide },
        { Direction.Bottom , TileSet.CellNeighbor.BottomLeftSide },
        { Direction.TopLeft , TileSet.CellNeighbor.TopCorner },
        { Direction.TopRight , TileSet.CellNeighbor.RightCorner },
        { Direction.BottomLeft , TileSet.CellNeighbor.LeftCorner },
        { Direction.BottomRight , TileSet.CellNeighbor.BottomCorner }
    };

    private static readonly IDictionary<int, Vector2I> NeighborAtlasMap = new Dictionary<int, Vector2I>
        {
            { 0, new Vector2I(0, 3) },
            { 1, new Vector2I(3, 3) },
            { 2, new Vector2I(0, 0) },
            { 3, new Vector2I(3, 2) },
            { 4, new Vector2I(0, 2) },
            { 5, new Vector2I(1, 2) },
            { 6, new Vector2I(2, 3) },
            { 7, new Vector2I(3, 1) },
            { 8, new Vector2I(1, 3) },
            { 9, new Vector2I(0, 1) },
            { 10, new Vector2I(3, 0) },
            { 11, new Vector2I(2, 0) },
            { 12, new Vector2I(1, 0) },
            { 13, new Vector2I(2, 2) },
            { 14, new Vector2I(1, 1) },
            { 15, new Vector2I(2, 1) },
        };

    private readonly HashSet<Vector2I> checkedCellCache = new();
    private readonly HashSet<Vector2I> emptiedCellCache = new();
    private readonly HashSet<Vector2I> filledCellCache = new();
        
    [Flags]
    enum Location
    {
        None = 0,
        TopLeft = 1 << 0,
        BottomLeft = 1 << 1,
        TopRight = 1 << 2,
        BottomRight = 1 << 3
    }

    enum TileSketchResult
    {
        Empty = -1,
        EmptyTile = 0,
        FilledTile = 1
    }

    enum Direction
    {
        Top,
        Left,
        Bottom,
        Right,
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight
    }

    private TileMapLayer displayTilemap;
    private bool isometricMode;
    private bool useCellCheckCache;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _Ready()
    {
        base._Ready();

        if (Engine.IsEditorHint())
        {
            SetProcess(true);
        }
        else
        {
            SetProcess(false);
            Changed += OnNodeChanged;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.TileSet == null)
        {
            return;
        }
        
        CallDeferred(nameof(UpdateTileset));
    }

    public void Erase(Vector2I cell, int atlasId = 0)
    {
        this.InitializeIfNeeded();
        
        this.SetCell(cell, atlasId, EmptyTile);
        this.UpdateTile(cell);
    }

    public void Fill(Vector2I cell, int atlasId = 0)
    {
        this.InitializeIfNeeded();
        
        this.SetCell(cell, atlasId, FullTile);
        this.UpdateTile(cell);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void InitializeIfNeeded()
    {
        if (this.displayTilemap == null || this.displayTilemap.TileSet != this.TileSet)
        {
            this.InitializeDisplay();
        }
    }
    
    private void OnNodeChanged()
    {
        UpdateTileset();
    }

    private void InitializeDisplay()
    {
        if (this.TileSet == null)
        {
            return;
        }

        if (this.Material == null)
        {
            var canvasMaterial = new CanvasItemMaterial();
            canvasMaterial.LightMode = CanvasItemMaterial.LightModeEnum.LightOnly;
            this.Material = canvasMaterial;
        }

        if (GetNodeOrNull(TileMapDisplayNodeName) == null)
        {
            this.displayTilemap = new TileMapLayer();
            this.displayTilemap.Name = TileMapDisplayNodeName;
            AddChild(this.displayTilemap);
        }

        if (this.displayTilemap.TileSet != this.TileSet)
        {
            this.displayTilemap.TileSet = this.TileSet;
        }

        this.isometricMode = this.TileSet.TileShape == TileSet.TileShapeEnum.Isometric;
        this.displayTilemap.Position = this.isometricMode 
            ? -new Vector2(0, this.TileSet.TileSize.Y * 0.5f) 
            : -new Vector2(this.TileSet.TileSize.X * 0.5f, this.TileSet.TileSize.Y * 0.5f);
        
        this.displayTilemap.Clear();
    }

    private void UpdateTilesetFull()
    {
        this.InitializeIfNeeded();

        this.useCellCheckCache = true;
        var usedCells = this.GetUsedCells();
        for (var i = 0; i < usedCells.Count; i++)
        {
            var cell = usedCells[i];
            TileSketchResult sketch = GetTileSketchStatus(cell);
            switch (sketch)
            {
                case TileSketchResult.EmptyTile:
                case TileSketchResult.FilledTile:
                {
                    UpdateTile(cell);
                    break;
                }
            }
        }

        this.useCellCheckCache = false;
    }

    private void UpdateTileset()
    {
        this.InitializeIfNeeded();

        var newEmptyCells = GetUsedCellsById(-1, EmptyTile);
        var newFilledCells = GetUsedCellsById(-1, FullTile);

        using var changedCells = TempHashSet<Vector2I>.Allocate();
        for (var i = 0; i < newFilledCells.Count; i++)
        {
            var cell = newFilledCells[i];
            if (!this.filledCellCache.Add(cell))
            {
                continue;
            }

            changedCells.Add(cell);
        }
        
        for (var i = 0; i < newEmptyCells.Count; i++)
        {
            var cell = newEmptyCells[i];
            if (!this.emptiedCellCache.Add(cell))
            {
                continue;
            }

            changedCells.Add(cell);
        }

        if (changedCells.Count <= 0)
        {
            return;
        }
        
        GD.Print("Updating " + changedCells.Count + " cells");
        foreach (Vector2I cell in changedCells)
        {
            UpdateTile(cell);
        }
    }

    private void UpdateTile(Vector2I cell, bool recursive = true)
    {
        var atlasId = GetCellSourceId(cell);

        if (!recursive && atlasId == -1)
        {
            return;
        }

        var neighbors = isometricMode ? NeighborMapIsometric : NeighborMap;
        var topLeft = cell;
        var bottomLeft = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.Bottom]);
        var topRight = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.Right]);
        var bottomRight = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.BottomRight]);

        UpdateTileDisplay(topLeft, atlasId);
        UpdateTileDisplay(bottomLeft, atlasId);
        UpdateTileDisplay(topRight, atlasId);
        UpdateTileDisplay(bottomRight, atlasId);

        if (atlasId != -1)
        {
            return;
        }
        
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.Left]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.TopLeft]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.Top]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.TopRight]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.Right]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.BottomRight]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.Bottom]), false);
        UpdateTile(this.GetNeighborCell(cell, neighbors[Direction.BottomLeft]), false);
    }

    private void UpdateTileDisplay(Vector2I cell, int atlasId)
    {
        if (this.useCellCheckCache)
        {
            if (this.checkedCellCache.Contains(cell))
            {
                return;
            }

            this.checkedCellCache.Add(cell);
        }
        
        var neighbors = isometricMode ? NeighborMapIsometric : NeighborMap;
        var topLeft = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.TopLeft]);
        var bottomLeft = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.Left]);
        var topRight = this.displayTilemap.GetNeighborCell(cell, neighbors[Direction.Top]);
        var bottomRight = cell;

        Location tileLocation = 0;
        if(GetTileSketchStatus(topLeft) == TileSketchResult.FilledTile) { tileLocation |= Location.TopLeft;}
        if(GetTileSketchStatus(bottomLeft) == TileSketchResult.FilledTile) { tileLocation |= Location.BottomLeft;}
        if(GetTileSketchStatus(topRight) == TileSketchResult.FilledTile) { tileLocation |= Location.TopRight;}
        if(GetTileSketchStatus(bottomRight) == TileSketchResult.FilledTile) { tileLocation |= Location.BottomRight;}

        var atlasCoordinates = NeighborAtlasMap[(int)tileLocation];
        this.displayTilemap.SetCell(cell, atlasId, atlasCoordinates);
    }

    private TileSketchResult GetTileSketchStatus(Vector2I cell)
    {
        var atlasCoordinates = GetCellAtlasCoords(cell);
        if (atlasCoordinates == FullTile)
        {
            return TileSketchResult.FilledTile;
        }

        if (atlasCoordinates == EmptyTile)
        {
            return TileSketchResult.EmptyTile;
        }

        return TileSketchResult.Empty;
    }
}