namespace Craiel.Essentials.Data;

using Godot;

public readonly struct DraggableSpriteViewport
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public Vector2 RegionOffset { get; }
    public float ZoomLevel { get; }

    public DraggableSpriteViewport(Vector2 regionOffset, float zoomLevel = 1.0f)
    {
        this.RegionOffset = regionOffset;
        this.ZoomLevel = zoomLevel;
    }

    public Vector2 SpriteToScreen(Vector2 position)
    {
        return (position - this.RegionOffset) * this.ZoomLevel;
    }

    public Vector2 ScreenToSprite(Vector2 screenPosition)
    {
        return (screenPosition / this.ZoomLevel) + this.RegionOffset;
    }
}