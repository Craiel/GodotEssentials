namespace Craiel.Essentials.Nodes;

using Godot;

public partial class SpriteShakeNode : Node
{
    private Vector2 spriteDefaultPosition;
    private Color spriteDefaultModulate;
    
    private bool isShaking;
    private double time;
    private float intensity;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public Sprite2D Target;
    [Export] public float Offset = 40f;
    [Export] public float Duration = 0.15f;
    [Export] public Color ModulateTint = new(1f, 0.75f, 0.75f);

    public override void _EnterTree()
    {
        base._EnterTree();

        this.ResetPositions();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!this.isShaking)
        {
            return;
        }
        
        if (this.time > 0f)
        {
            this.time -= delta;
            this.UpdateShake();
            return;
        }
        
        this.isShaking = false;
        this.Target.Position = this.spriteDefaultPosition;
        this.Target.Modulate = this.spriteDefaultModulate;
    }

    public void ResetPositions()
    {
        this.spriteDefaultPosition = this.Target.Position;
        this.spriteDefaultModulate = this.Target.Modulate;
    }

    public void Shake(float newIntensity)
    {
        // Update the intensity if its bigger and reset the duration
        if (this.intensity < newIntensity)
        {
            this.intensity = newIntensity;
        }

        this.isShaking = true;
        this.time = this.Duration;
    }
    
    private void UpdateShake()
    {
        float minShake = this.Offset * -this.intensity;
        float maxShake = this.Offset * this.intensity;
        Vector2 shakeOffset = new Vector2(
            EssentialCore.Random.RandfRange(minShake, maxShake),
            EssentialCore.Random.RandfRange(minShake, maxShake));
		
        this.Target.Position = this.spriteDefaultPosition + shakeOffset;

        float colorMod = this.ModulateTint.R - EssentialCore.Random.RandfRange(0, this.intensity);
        this.Target.Modulate = new Color(colorMod, this.ModulateTint.G, ModulateTint.B);
    }
}