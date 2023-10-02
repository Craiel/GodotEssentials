namespace Craiel.Essentials;

public class DynamicAudioStreamStateFadeOut : DynamicAudioStreamStateBase
{
    public static readonly DynamicAudioStreamStateFadeOut Instance = new DynamicAudioStreamStateFadeOut();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Enter(DynamicAudioStream entity)
    {
        base.Enter(entity);

        entity.CurrentFadeTime = 0;
    }

    public override void Update(double delta, DynamicAudioStream entity)
    {
        base.Update(delta, entity);

        entity.CurrentFadeTime += delta;
        if (entity.CurrentFadeTime < entity.Parameters.FadeOut)
        {
            SetVolume(entity, 1f - ((float)entity.CurrentFadeTime / entity.Parameters.FadeOut));
            return;
        }

        SetVolume(entity, 0f);
        entity.SwitchState(DynamicAudioStreamState.Finished);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void SetVolume(DynamicAudioStream entity, float volume)
    {
        entity.VolumeDb = volume;
    }
}
