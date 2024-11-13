namespace Craiel.Essentials.Nodes;

using Godot;

public partial class ToggleDisplayButtonNode : Button
{
    [Export] public Control Target;
    
    public override void _Ready()
    {
        base._Ready();

        if (this.Target == null)
        {
            EssentialCore.Logger.Error("Toggle Display Mode Button has no target set!");
        }

        this.Toggled += OnToggle;
        
        // Call the initial on toggle to the current state
        this.OnToggle(this.ButtonPressed);
    }

    private void OnToggle(bool value)
    {
        if (value)
        {
            this.Target.Show();
        }
        else
        {
            this.Target.Hide();
        }
    }
}