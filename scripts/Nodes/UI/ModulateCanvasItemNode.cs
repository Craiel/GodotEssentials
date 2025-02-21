namespace Craiel.Essentials.Nodes;

using Godot;
using Colors = Extensions.Colors;

public partial class ModulateCanvasItemNode : Node, ISynchronizedNode
{
    private Color originalColor;
    private Color startColor;
    private Color currentColor;
    private bool inProgress;
    private bool isReverse;
    private double currentTime;
    private double currentDelay;

    private bool resyncRequired;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public CanvasItem Target;
    [Export] public float Duration;
    [Export] public float Delay;
    [Export] public bool Loop;
    [Export] public bool Synchronized;
    [Export] public bool BlendAlpha = true;
    [Export] public bool BlendColor = true;
    [Export] public bool RestoreWhenFinished = true;
    [Export] public Color Color = Colors.White;
    

    public override void _EnterTree()
    {
        base._EnterTree();

        if (this.Synchronized)
        {
            SynchronizerNode.Instance?.Register(this);
        }
    }

    public override void _ExitTree()
    {
        if (this.Synchronized)
        {
            SynchronizerNode.Instance?.Unregister(this);
        }

        base._ExitTree();
    }

    public override void _Ready()
    {
        base._Ready();

        this.originalColor = this.Target.Modulate;
        this.currentColor = this.originalColor;
        this.TargetColor = this.Color;
        
        this.Begin();
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!this.Target.IsVisibleInTree())
        {
            this.resyncRequired = true;
            return;
        }

        if (this.resyncRequired && this.Synchronized)
        {
            this.resyncRequired = false;
            SynchronizerNode.Instance?.Unregister(this);
            SynchronizerNode.Instance?.Register(this);
        }

        if (this.currentDelay > 0)
        {
            this.currentDelay -= delta;
            return;
        }

        if (this.inProgress)
        {
            this.currentTime += delta;
            this.Continue();
            return;
        }

        if (this.Loop)
        {
            // Start another round
            this.isReverse = !this.isReverse;
            this.Begin();
        }
    }
    
    public void _ProcessSynchronized(double delta)
    {
    }

    public virtual void _ResetSynchronized()
    {
        this.Begin();
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected Color TargetColor;
    
    protected virtual void Begin()
    {
        this.inProgress = true;
        this.currentTime = 0f;
        this.currentDelay = this.Delay;

        this.currentColor = this.Target.Modulate;
        this.startColor = this.currentColor;
        this.TargetColor = this.isReverse ? this.originalColor : this.Color;
    }
    
    protected virtual void Continue()
    {
        float progress = Mathf.Clamp((float)this.currentTime / this.Duration, 0, 1);
        
        if (this.BlendColor)
        {
            this.currentColor = this.startColor.Lerp(this.TargetColor, progress);
            if (!this.BlendAlpha)
            {
                this.currentColor.A = this.startColor.A;
            }
        }
        else
        {
            this.currentColor = this.startColor;
            if (this.BlendAlpha)
            {
                this.currentColor.A = Mathf.Lerp(this.startColor.A, this.TargetColor.A, progress);
            }
        }
        
        this.Target.Modulate = this.currentColor;
        
        if (this.currentTime >= this.Duration)
        {
            this.inProgress = false;
            if (this.Loop && this.RestoreWhenFinished)
            {
                // Restore the modulation, we are looping the mode but not alternating
                this.Target.Modulate = this.originalColor;
            }
        }
    }
}