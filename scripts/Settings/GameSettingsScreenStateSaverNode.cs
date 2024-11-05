namespace Craiel.Essentials.Settings;

using Godot;

public partial class GameSettingsScreenStateSaverNode : Node
{
    private double timeSinceLastSave;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public float SaveInterval = 60;
    
    public override void _Ready()
    {
        base._Ready();

        this.GetTree().Root.SizeChanged += OnWindowSizeChanged;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        this.timeSinceLastSave += delta;
        if (this.timeSinceLastSave >= this.SaveInterval)
        {
            this.timeSinceLastSave = 0;
            this.SavePosition();
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void OnWindowSizeChanged()
    {
        this.SavePosition();
        GameSettingsVideo.SetWindowSize(this.GetTree().Root.Size, apply: false);
    }

    private void SavePosition()
    {
        GameSettingsVideo.SetPosition(this.GetTree().Root.Position, apply: false);
    }
}