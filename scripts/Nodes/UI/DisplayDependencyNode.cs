namespace Craiel.Essentials.Nodes;

using Godot;

public partial class DisplayDependencyNode : Control
{
    [Export] public Control DependsOn;
    [Export] public bool HideOnly = true;

    public override void _Ready()
    {
        base._Ready();

        this.DependsOn.VisibilityChanged += OnDependantControlVisibilityChanged;
    }

    private void OnDependantControlVisibilityChanged()
    {
        if (this.DependsOn.Visible)
        {
            if (!this.HideOnly)
            {
                this.Show();
            }
        }
        else
        {
            this.Hide();
        }
    }
}