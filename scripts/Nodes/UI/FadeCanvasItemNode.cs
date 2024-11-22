namespace Craiel.Essentials.Nodes;

using Godot;

public partial class FadeCanvasItemNode : ModulateCanvasItemNode
{
    private static readonly Color FadeOutColor = new(1, 1, 1, 0);
    private static readonly Color FadeInColor = new(1, 1, 1);
    
    private FadeMode currentMode;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public enum FadeMode
    {
        FadeOut,
        FadeIn,
        Alternate
    }
    
    [Export] public FadeMode Mode = FadeMode.FadeOut;

    public override void _Ready()
    {
        base._Ready();

        if (this.Mode == FadeMode.Alternate)
        {
            this.RestoreWhenFinished = false;
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    protected override void Begin()
    {
        switch (this.Mode)
        {
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

            default:
            {
                this.currentMode = this.Mode;
                break;
            }
        }

        // First reset the modulate, then call begin and let it know which target color we want to go to
        this.Target.Modulate = this.currentMode == FadeMode.FadeIn ? FadeOutColor : FadeInColor;
        base.Begin();
        this.TargetColor = this.currentMode == FadeMode.FadeIn ? FadeInColor : FadeOutColor;
    }
}