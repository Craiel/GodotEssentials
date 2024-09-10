namespace Craiel.Essentials;

using Godot;

public partial class ToggleButtonGroupNode : Control
{
    private Button activeButton;
    
    [Export] public Button[] Buttons;
    [Export] public Button Default;

    public override void _Ready()
    {
        base._Ready();

        if (this.Buttons.Length <= 0)
        {
            EssentialCore.Logger.Error("Toggle Button Group without any buttons!");
            return;
        }
        
        for (var i = 0; i < this.Buttons.Length; i++)
        {
            var button = this.Buttons[i];
            button.Toggled += x =>
            {
                OnButtonToggled(button, x);
            };
        }

        if (this.Default != null)
        {
            this.Default.ButtonPressed = true;
            this.activeButton = this.Default;
        }
        else
        {
            this.Buttons[0].ButtonPressed = true;
            this.activeButton = this.Default;
        }
    }

    private void OnButtonToggled(Button button, bool value)
    {
        if (this.activeButton == button)
        {
            if (!value)
            {
                // Prevent turning off buttons in a group
                button.ButtonPressed = true;
                return;
            }
            
            return;
        }

        if (!value)
        {
            return;
        }

        Button previousActiveButton = this.activeButton;
        this.activeButton = button;

        if (previousActiveButton != null)
        {
            previousActiveButton.ButtonPressed = false;
        }
    }
}