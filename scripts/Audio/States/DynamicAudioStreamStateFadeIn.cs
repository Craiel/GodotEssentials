namespace Craiel.Essentials;

public class DynamicAudioStreamStateFadeIn : DynamicAudioStreamStateBase
{
    public static readonly DynamicAudioStreamStateFadeIn Instance = new DynamicAudioStreamStateFadeIn();

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
        if (entity.CurrentFadeTime < entity.Parameters.FadeIn)
        {
            SetVolumeAndPlay(entity, (float)entity.CurrentFadeTime / entity.Parameters.FadeIn);
            return;
        }

        SetVolumeAndPlay(entity, 1f);
        entity.SwitchState(DynamicAudioStreamState.Playing);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void SetVolumeAndPlay(DynamicAudioStream entity, float volume)
    {
        entity.VolumeDb = volume;
        if (!entity.Playing)
        {
            entity.Play();
        }
    }
}
