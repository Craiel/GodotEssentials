namespace Craiel.Essentials.Nodes;

using Godot;

public partial class CollisionRadiusCheck2DNode : CollisionCheck2DNode
{
    private const float RadiusIncrement = 2 * Mathf.Pi / 60;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public float Radius = 200f;

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void BeginCollisionCheck()
    {
        this.RaycastArc(this.Target.GlobalPosition, this.Radius, RadiusIncrement, 2 * Mathf.Pi, RadiusIncrement);
    }
}