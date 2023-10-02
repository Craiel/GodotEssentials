namespace Craiel.Essentials;

public class DynamicAudioStreamStatePlaying : DynamicAudioStreamStateBase
{
    public static readonly DynamicAudioStreamStatePlaying Instance = new DynamicAudioStreamStatePlaying();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Update(double delta, DynamicAudioStream entity)
    {
        base.Update(delta, entity);

        if (!entity.Playing)
        {
            entity.SwitchState(DynamicAudioStreamState.Finished);
        }
    }
}