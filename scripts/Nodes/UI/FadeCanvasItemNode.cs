namespace Craiel.Essentials.Nodes.UI;

using Godot;

public partial class FadeCanvasItemNode : Node
{
    private Color defaultTargetModulate;
    private FadeMode currentMode;
    private bool fadeInProgress;
    private double currentFadeTime;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public enum FadeMode
    {
        FadeOut,
        FadeIn,
        Alternate
    }
    
    [Export] public CanvasItem Target;
    [Export] public float Duration;
    [Export] public bool Loop;
    [Export] public FadeMode Mode = FadeMode.FadeOut;

    public override void _Ready()
    {
        base._Ready();

        this.defaultTargetModulate = this.Target.Modulate;
        
        this.BeginFade();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.fadeInProgress)
        {
            this.currentFadeTime += delta;
            this.ContinueFade();
            return;
        }

        if (this.Loop)
        {
            // Start another round
            this.BeginFade();
        }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void ContinueFade()
    {
        float progress = Mathf.Clamp((float)this.currentFadeTime / this.Duration, 0, 1);
        if (this.currentMode == FadeMode.FadeIn)
        {
            progress = 1 - progress;
        }
        
        this.Target.Modulate = new Color(1, 1, 1, progress);
        
        if (this.currentFadeTime >= this.Duration)
        {
            this.fadeInProgress = false;
            if (this.Loop && this.Mode != FadeMode.Alternate)
            {
                // Restore the modulation, we are looping the mode but not alternating
                this.Target.Modulate = this.defaultTargetModulate;
            }
        }
    }

    private void BeginFade()
    {
        this.fadeInProgress = true;
        this.currentFadeTime = 0f;
        
        switch (this.Mode)
        {
            case FadeMode.FadeIn:
            case FadeMode.FadeOut:
            {
                this.currentMode = this.Mode;
                break;
            }

            case FadeMode.Alternate:
            {
                switch (this.currentMode)
                {
                    case FadeMode.FadeIn:
                    {
                        this.currentMode = FadeMode.FadeOut;
                        break;
                    }

                    case FadeMode.FadeOut:
                    {
                        this.currentMode = FadeMode.FadeIn;
                        break;
                    }

                    default:
                    {
                        this.currentMode = FadeMode.FadeOut;
                        break;
                    }
                }
                
                break;
            }
        }
    }
}