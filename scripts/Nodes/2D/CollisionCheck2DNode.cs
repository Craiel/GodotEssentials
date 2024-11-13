namespace Craiel.Essentials.Nodes;

using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract partial class CollisionCheck2DNode : Node2D
{
    private PhysicsDirectSpaceState2D physicsSpace;
    private PhysicsRayQueryParameters2D physicsParams = new();
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public PhysicsBody2D Target;
    [Export] public Polygon2D DisplayPolygon;
    [Export] public Vector2 MoveThreshold = new Vector2(0.1f, 0.1f);
    
    public IList<CollisionObject2D> HitObjects { get; private set; }

    public override void _EnterTree()
    {
        base._EnterTree();

        this.HitPointList = new List<Vector2>();
        this.HitObjects = new List<CollisionObject2D>();
    }

    public override void _Ready()
    {
        base._Ready();

        this.physicsSpace = PhysicsServer2D.SpaceGetDirectState(this.GetWorld2D().Space);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.Target != null && this.TargetPosition != this.Target.Position)
        {
            this.TargetPosition = this.Target.Position;
            this.RecalculateCollisions();
        }
    }

    public void Recheck()
    {
        this.RecalculateCollisions();
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected IList<Vector2> HitPointList { get; private set; }
    protected Vector2 TargetPosition { get; private set; }
    
    protected abstract void BeginCollisionCheck();

    protected void RaycastArc(Vector2 from, float radius, float startAngle, float endAngle, float increments)
    {
        float angle = startAngle;
        this.physicsParams.From = from;

        this.HitPointList.Clear();
        this.HitObjects.Clear();
        
        while (angle < endAngle)
        {
            var offset = new Vector2(radius, 0).Rotated(angle);
            this.physicsParams.To = from + offset;
            var result = this.physicsSpace.IntersectRay(this.physicsParams);
            if (result is { Count: > 0 })
            {
                this.HitObjects.Add(result["collider"].As<CollisionObject2D>());
                this.HitPointList.Add(result["position"].AsVector2() - from);
            }
            else
            {
                this.HitPointList.Add(this.physicsParams.To - from);
            }
            
            angle += increments;
        }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void RecalculateCollisions()
    {
        this.BeginCollisionCheck();

        if (this.DisplayPolygon is { Visible: true })
        {
            this.DisplayPolygon.Polygon = this.HitPointList.ToArray();
        }
    }
}