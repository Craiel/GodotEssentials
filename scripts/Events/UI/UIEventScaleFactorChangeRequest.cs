namespace Craiel.Essentials.Events.UI;

using Event;

public class UIEventScaleFactorChangeRequest : IUIEvent
{
    public UIEventScaleFactorChangeRequest(float value)
    {
        this.Value = value;
    }

    public float Value;
}