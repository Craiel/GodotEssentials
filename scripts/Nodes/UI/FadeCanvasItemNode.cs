namespace Craiel.Essentials.Nodes.UI;

using Godot;

public partial class FadeCanvasItemNode : ModulateCanvasItemNode
{
    private static readonly Color FadeOutColor = new Color(1, 1, 1, 0);
    private static readonly Color FadeInColor = new Color(1, 1, 1);
    
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
        base.Begin();
        
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

        this.TargetColor = this.currentMode == FadeMode.FadeIn ? FadeInColor : FadeOutColor;
    }
}