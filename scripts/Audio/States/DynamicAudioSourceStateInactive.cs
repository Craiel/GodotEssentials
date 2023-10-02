namespace Craiel.Essentials;

public class DynamicAudioStreamStateFinished : DynamicAudioStreamStateBase
{
    public static readonly DynamicAudioStreamStateFinished Instance = new DynamicAudioStreamStateFinished();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Update(double delta, DynamicAudioStream entity)
    {
        entity.Reset();
    }
}
