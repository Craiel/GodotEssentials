namespace CanineJRPG.godot.source.GodotEssentials.scripts.Nodes.UI;

using System;
using Craiel.Essentials.Enums;
using Godot;

public abstract partial class UIElementWithTransition : Control
{
    private Tween activeTransitionTween;
    private UIElementTransition activeTransition;
    private double autoHideTime = 0;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public float TransitionDuration = 0.2f;

    [ExportCategory("Auto Hide")]
    [Export] public bool AutoHide = false;
    [Export] public double AutoHideDelay = 4;
    
    public bool InTransition { get; private set; }
    
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!this.AutoHide || !this.Visible || this.InTransition)
        {
            return;
        }

        this.autoHideTime += delta;
        if (this.autoHideTime >= this.AutoHideDelay)
        {
            this.BeginTransition(UIElementTransition.Hide);
        }
    }

    public void BeginTransition(UIElementTransition transition, Action completed = null)
    {
        if (this.InTransition && this.activeTransition == transition)
        {
            // Already in transition
            return;
        }

        this.SetVisible(true);
        this.InTransition = true;
        this.activeTransition = transition;
        this.autoHideTime = 0;

        if (this.activeTransitionTween != null && this.activeTransitionTween.IsRunning())
        {
            this.activeTransitionTween.Kill();
            this.activeTransitionTween = null;
        }
        
        this.activeTransitionTween = this.ExecuteTransition(transition);
        this.activeTransitionTween.Finished += () =>
        {
            OnTransitionComplete();
            if (completed != null)
            {
                completed.Invoke();
            }
        };
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected abstract Tween ExecuteTransition(UIElementTransition transition);
    
    protected virtual void OnTransitionComplete()
    {
        this.InTransition = false;
        if (this.activeTransition == UIElementTransition.Hide)
        {
            this.SetVisible(false);
        }
    }
}