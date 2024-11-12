namespace Craiel.Essentials;

using Godot;

public partial class CollisionConeCheck2DNode : CollisionCheck2DNode
{
    private const float RadiusIncrement = 2 * Mathf.Pi / 60;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public float Radius = 200f;
    [Export] public float Width = 1;
    [Export] public Vector2 Forward;

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected override void BeginCollisionCheck()
    {
        float forwardAngle = this.Forward.Angle();
        float startAngle = forwardAngle - this.Width / 2;
        float endAngle = forwardAngle + this.Width / 2;
        this.RaycastArc(this.Target.GlobalPosition, this.Radius, startAngle, endAngle, RadiusIncrement);
        this.HitPointList.Add(Vector2.Zero);
    }
}