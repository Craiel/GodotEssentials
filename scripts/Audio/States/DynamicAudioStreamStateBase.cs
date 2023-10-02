namespace Craiel.Essentials;

using Contracts;
using Msg;

public class DynamicAudioStreamStateBase : IState<DynamicAudioStream>
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public virtual void Enter(DynamicAudioStream entity)
    {
    }

    public virtual void Update(double delta, DynamicAudioStream entity)
    {
    }

    public void Exit(DynamicAudioStream entity)
    {
    }

    public bool OnMessage(DynamicAudioStream entity, Telegram telegram)
    {
        return false;
    }
}