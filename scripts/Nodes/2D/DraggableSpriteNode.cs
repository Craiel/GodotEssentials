namespace Craiel.Essentials.Nodes;

using System;
using Godot;

public partial class DraggableSpriteNode : Sprite2D
{
    [Export] public float DragSpeed = 0.5f;

    [Export] public double AutoCenterTime = 5f;
    [Export] public float AutoCenterMargin = 10f;

    private bool isDragging;
    private bool isAutoCentering;
    private double elapsedAutoCenterTime;
    private Vector2 dragOffset;

    private Vector2 regionSize;
    private Vector2 regionHalfSize;
    private Vector2 regionOffset;
    private Vector2 textureSize;
    private Vector2 maxRegionOffset;
    private Vector2 autoCenterTarget;

    public event Action RegionChanged;
    
    public Vector2 RegionOffset => this.regionOffset;

    public override void _Ready()
    {
        base._Ready();

        // Store the initial Region Rect position
        this.regionSize = this.RegionRect.Size;
        this.regionHalfSize = new Vector2(this.regionSize.X / 2f, this.regionSize.Y / 2f);
        this.regionOffset = this.RegionRect.Position;

        this.textureSize = new Vector2(this.Texture.GetWidth(), this.Texture.GetHeight());

        var rectSize = this.GetRect().Size;
        this.maxRegionOffset = new Vector2(
            Mathf.Max(0, this.textureSize.X - rectSize.X),
            Mathf.Max(0, this.textureSize.Y - rectSize.Y));
    }

    public override void _Input(InputEvent @event)
    {
        if (!this.Visible || this.isAutoCentering)
        {
            // Ignore any input while we are not visible or auto centering
            return;
        }
        
        if (@event is InputEventMouseButton mouseEvent)
        {
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
                // Stop dragging
                this.isDragging = false;
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

    private bool TryMoveRegion(Vector2 newOffset)
    {
        // Clamp region offset to keep the image within the rect
        newOffset = new Vector2(
            Mathf.Max(0, Mathf.Min(this.maxRegionOffset.X, newOffset.X)),
            Mathf.Max(0, Mathf.Min(this.maxRegionOffset.Y, newOffset.Y)));
        
        if (this.regionOffset == newOffset)
        {
            return false;
        }

        this.regionOffset = newOffset;
        this.RegionRect = new Rect2(this.regionOffset, this.regionSize);
            
        this.RegionChanged?.Invoke();
        return true;
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
}