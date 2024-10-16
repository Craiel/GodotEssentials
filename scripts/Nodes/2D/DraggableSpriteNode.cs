namespace Craiel.Essentials;

using Godot;

public partial class DraggableSpriteNode : Sprite2D
{
    [Export]
    public float Speed = 1f;

    private bool isDragging = false;
    private Vector2 dragOffset;

    private Vector2 regionSize;
    private Vector2 regionOffset;
    private Vector2 textureSize;
    private Vector2 maxRegionOffset;

    public override void _Ready()
    {
        base._Ready();

        // Store the initial Region Rect position
        this.regionSize = this.RegionRect.Size;
        this.regionOffset = this.RegionRect.Position;

        this.textureSize = new Vector2(this.Texture.GetWidth(), this.Texture.GetHeight());

        var rectSize = this.GetRect().Size;
        this.maxRegionOffset = new Vector2(
            Mathf.Max(0, this.textureSize.X - rectSize.X),
            Mathf.Max(0, this.textureSize.Y - rectSize.Y));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left &&
                this.GetRect().HasPoint(ToLocal(mouseEvent.GlobalPosition)))
            {
                // Start dragging
                this.isDragging = true;
                this.dragOffset = mouseEvent.GlobalPosition;
            }
            else if (!mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                // Stop dragging
                this.isDragging = false;
            }
        }

        if (this.isDragging && @event is InputEventMouseMotion motionEvent)
        {
            // Continue dragging
            Vector2 diff = (this.dragOffset - motionEvent.GlobalPosition) * Speed;
            this.dragOffset = motionEvent.GlobalPosition;
            this.regionOffset += diff;

            // Clamp region offset to keep the image within the rect
            this.regionOffset = new Vector2(
                Mathf.Max(0, Mathf.Min(this.maxRegionOffset.X, this.regionOffset.X)),
                Mathf.Max(0, Mathf.Min(this.maxRegionOffset.Y, this.regionOffset.Y)));
            
            this.RegionRect = new Rect2(this.regionOffset, this.regionSize);
        }
    }
}