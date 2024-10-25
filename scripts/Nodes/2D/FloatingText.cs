namespace Craiel.Essentials;

using Enums;
using Godot;

public abstract partial class FloatingText : Control
{
    private Tween tween;
    private bool textSet;
    private bool tweenStarted;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public Label Text;
    [Export] public float Duration = 0.5f;

    public override void _EnterTree()
    {
        base._EnterTree();

        // Start hidden, only after set text will we initialize
        this.Hide();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!this.tweenStarted && this.textSet)
        {
            this.BeginTween();
        }
    }

    public void SetText(string text, Color color, TextFlags flags = TextFlags.None)
    {
        this.Text.Text = text;
        this.Text.LabelSettings.FontColor = color;

        if ((flags & TextFlags.Uppercase) != 0)
        {
            this.Text.Uppercase = true;
        }

        this.textSet = true;
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected virtual Tween DoBeginTween()
    {
        var result = this.GetTree().CreateTween();
        result.SetParallel();
        result.TweenProperty(this, new NodePath("scale"), Vector2.Zero, this.Duration)
            .SetEase(Tween.EaseType.In).SetDelay(this.Duration);
        return result;
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void BeginTween()
    {
        this.Show();
        
        this.tweenStarted = true;

        this.tween = this.DoBeginTween();
        this.tween.Finished += OnTweenFinished;
    }

    private void OnTweenFinished()
    {
        // Queue deletion
        this.QueueFree();
    }
}