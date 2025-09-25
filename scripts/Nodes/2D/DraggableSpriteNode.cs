namespace Craiel.Essentials.Nodes;

using System;
using Craiel.Essentials.Data;
using Godot;

public partial class DraggableSpriteNode : Sprite2D
{
    private bool isDragging;
    private bool isAutoCentering;
    private double elapsedAutoCenterTime;
    private Vector2 dragOffset;

    private Vector2 baseRegionSize;
    private Vector2 regionSize;
    private Vector2 regionHalfSize;
    private Vector2 regionOffset;
    private Vector2 textureSize;
    private Vector2 maxRegionOffset;
    private Vector2 autoCenterTarget;
    private float zoomLevel = 1.0f;

    private DraggableSpriteViewport currentViewport;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [ExportCategory("Dragging")]
    [Export] public float DragSpeed = 0.5f;

    [ExportCategory("Auto Centering")]
    [Export] public double AutoCenterTime = 5f;
    [Export] public float AutoCenterMargin = 10f;
    
    [ExportCategory("Zoom")]
    [Export] public bool EnableScrollWheelZoom;
    [Export] public float ZoomDefault = 1.0f;
    [Export] public float MinZoom = 0.5f;
    [Export] public float MaxZoom = 3.0f;
    [Export] public float ZoomStep = 0.5f;
    
    public event Action RegionChanged;
    
    public Vector2 RegionOffset => this.regionOffset;

    public DraggableSpriteViewport GetCurrentViewport() => this.currentViewport;

    public override void _Ready()
    {
        base._Ready();

        // Store the initial Region Rect position
        this.baseRegionSize = this.RegionRect.Size;
        this.regionOffset = this.RegionRect.Position;

        this.textureSize = new Vector2(this.Texture.GetWidth(), this.Texture.GetHeight());

        this.zoomLevel = this.ZoomDefault;
        
        this.UpdateRegionForZoom();
        this.UpdateMaxRegionOffset();
        this.UpdateViewport();
    }

    public void StopDragging()
    {
        this.isDragging = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (!this.IsVisibleInTree() || this.isAutoCentering)
        {
            // Ignore any input while we are not visible or auto centering
            return;
        }
        
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (this.EnableScrollWheelZoom && mouseEvent.ButtonIndex == MouseButton.WheelUp && mouseEvent.Pressed)
            {
                this.ZoomIn();
                return;
            }

            if (this.EnableScrollWheelZoom && mouseEvent.ButtonIndex == MouseButton.WheelDown && mouseEvent.Pressed)
            {
                this.ZoomOut();
                return;
            }

            if (mouseEvent.ButtonIndex != MouseButton.Left && !this.isDragging)
            {
                // No longer interested in this event
                return;
            }
            
            var localMousePos = ToLocal(mouseEvent.GlobalPosition);
            if (mouseEvent.Pressed && this.GetRect().HasPoint(localMousePos))
            {
                // Start dragging
                this.isDragging = true;
                this.dragOffset = mouseEvent.GlobalPosition;
                
#if DEBUG
                // Adjust the mouse position to get it into a good coordinate space
                var clickCoordinate = localMousePos + this.regionHalfSize + this.regionOffset;
                EssentialCore.Logger.Info("DRAG_START: " + clickCoordinate);
#endif
                
            }
            else if (!mouseEvent.Pressed)
            {
                this.StopDragging();
            }
        }

        if (this.isDragging && @event is InputEventMouseMotion motionEvent)
        {
            // Continue dragging
            Vector2 diff = (this.dragOffset - motionEvent.GlobalPosition) * this.DragSpeed;
            this.dragOffset = motionEvent.GlobalPosition;

            // We don't care if this fails during drag
            this.TryMoveRegion(this.regionOffset + diff);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.isAutoCentering)
        {
            this.elapsedAutoCenterTime += delta;
            this.ContinueAutoCenter();
        }
    }

    public void CenterOn(Vector2 newTarget)
    {
        if (this.isDragging)
        {
            // We won't auto-center while being dragged
            return;
        }
        
        this.elapsedAutoCenterTime = 0f;
        this.autoCenterTarget = newTarget - this.regionHalfSize;
        this.isAutoCentering = true;
    }
    
    public void ZoomIn()
    {
        this.SetZoom(this.zoomLevel + this.ZoomStep);
    }

    public void ZoomOut()
    {
        this.SetZoom(this.zoomLevel - this.ZoomStep);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private bool TryMoveRegion(Vector2 newOffset)
    {
        // Calculate the valid range for region offset
        float minX = Mathf.Min(0, this.textureSize.X - this.regionSize.X);
        float maxX = Mathf.Max(0, this.textureSize.X - this.regionSize.X);
        float minY = Mathf.Min(0, this.textureSize.Y - this.regionSize.Y);
        float maxY = Mathf.Max(0, this.textureSize.Y - this.regionSize.Y);

        Vector2 clampedOffset = new Vector2(
            Mathf.Clamp(newOffset.X, minX, maxX),
            Mathf.Clamp(newOffset.Y, minY, maxY));

        if (this.regionOffset == clampedOffset)
        {
            return false;
        }

        this.regionOffset = clampedOffset;
        this.RegionRect = new Rect2(this.regionOffset, this.regionSize);

        this.UpdateViewport();
        this.RegionChanged?.Invoke();
        return true;
    }
    private void ContinueAutoCenter()
    {
        Vector2 newOffset = this.regionOffset.Lerp(this.autoCenterTarget, (float)Mathf.Min(this.elapsedAutoCenterTime / this.AutoCenterTime, 1f));
        float distanceToTarget = this.regionOffset.DistanceTo(autoCenterTarget);
        if (distanceToTarget <= this.AutoCenterMargin || !this.TryMoveRegion(newOffset))
        {
            // We could not move, abort any further updates
            this.isAutoCentering = false;
        }
    }

    private void UpdateViewport()
    {
        this.currentViewport = new DraggableSpriteViewport(this.regionOffset, this.zoomLevel);
    }

    private void SetZoom(float newZoom)
    {
        float clampedZoom = Mathf.Clamp(newZoom, this.MinZoom, this.MaxZoom);
        if (Mathf.IsEqualApprox(this.zoomLevel, clampedZoom))
        {
            return;
        }

        // Store current center before zoom change
        Vector2 currentCenter = this.regionOffset + (this.regionSize / 2f);

        this.zoomLevel = clampedZoom;
        this.Scale = new Vector2(this.zoomLevel, this.zoomLevel);
        this.UpdateRegionForZoom();
        this.UpdateMaxRegionOffset();

        // Use auto-center to smoothly move to the previous center
        this.CenterOn(currentCenter);

        // Update viewport immediately for new zoom
        this.UpdateViewport();
        this.RegionChanged?.Invoke();
    }

    private void UpdateRegionForZoom()
    {
        this.regionSize = this.baseRegionSize / this.zoomLevel;
        this.regionHalfSize = new Vector2(this.regionSize.X / 2f, this.regionSize.Y / 2f);
        this.RegionRect = new Rect2(this.regionOffset, this.regionSize);
    }

    private void UpdateMaxRegionOffset()
    {
        this.maxRegionOffset = new Vector2(
            Mathf.Max(0, this.textureSize.X - this.regionSize.X),
            Mathf.Max(0, this.textureSize.Y - this.regionSize.Y));
    }
}