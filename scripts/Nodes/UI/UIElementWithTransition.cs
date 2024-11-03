namespace CanineJRPG.godot.source.GodotEssentials.scripts.Nodes.UI;

using Craiel.Essentials.Enums;
using Godot;

public abstract partial class UIElementWithTransition : Control
{
    private Tween activeTransitionTween;
    private UIElementTransition activeTransition;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public float TransitionDuration;
    
    public bool InTransition { get; private set; }

    public void BeginTransition(UIElementTransition transition)
    {
        if (this.InTransition && this.activeTransition == transition)
        {
            // Already in transition
            return;
        }

        this.InTransition = true;
        this.activeTransition = transition;

        if (this.activeTransitionTween != null && this.activeTransitionTween.IsRunning())
        {
            this.activeTransitionTween.Kill();
            this.activeTransitionTween = null;
        }
        
        this.activeTransitionTween = this.ExecuteTransition(transition);
    }
    
    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected abstract Tween ExecuteTransition(UIElementTransition transition);
}