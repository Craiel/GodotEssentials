namespace Craiel.Essentials;

using Contracts;
using EngineCore;
using FSM;
using GameData;
using Godot;
using Resource;

public partial class DynamicAudioStream : AudioStreamPlayer, IPoolable, ITicketData
{
	private readonly EnumStateMachine<DynamicAudioStream, DynamicAudioStreamStateBase, DynamicAudioStreamState> state;
	
	// -------------------------------------------------------------------
	// Constructor
	// -------------------------------------------------------------------
	public DynamicAudioStream()
	{
		this.state = new EnumStateMachine<DynamicAudioStream, DynamicAudioStreamStateBase, DynamicAudioStreamState>(this, DynamicAudioStreamStateInactive.Instance);
		this.state.SetState(DynamicAudioStreamState.Inactive, DynamicAudioStreamStateInactive.Instance);
		this.state.SetState(DynamicAudioStreamState.FadeIn, DynamicAudioStreamStateFadeIn.Instance);
		this.state.SetState(DynamicAudioStreamState.FadeOut, DynamicAudioStreamStateFadeOut.Instance);
		this.state.SetState(DynamicAudioStreamState.Playing, DynamicAudioStreamStatePlaying.Instance);
		this.state.SetState(DynamicAudioStreamState.Finished, DynamicAudioStreamStateFinished.Instance);
	}

	// -------------------------------------------------------------------
	// Public
	// -------------------------------------------------------------------
	public AudioPlayParameters Parameters;

	public double CurrentFadeTime;

	public GameDataId ActiveId { get; private set; }

	public AudioTicket Ticket { get; private set; }

	public bool IsActive
	{
		get { return this.state.CurrentState != DynamicAudioStreamStateFinished.Instance; }
	}

	public void Reset()
	{
		this.Stream = null;

		this.Parameters = default;

		this.ActiveId = GameDataId.Invalid;

		this.state.SwitchState(DynamicAudioStreamState.Inactive);
	}

	public void Update(double delta)
	{
		this.state.Update(delta);
	}

	public void Start(AudioTicket ticket, RuntimeAudioData entry, bool is3D, AudioPlayParameters parameters)
	{
		this.Stream = parameters.UseRandomClip
			? this.GetClip(entry, (ushort)EssentialCore.Random.RandiRange(0, entry.ClipKeys.Count))
			: this.GetClip(entry, parameters.ClipIndex);
		
		this.Parameters = parameters;

		this.ActiveId = entry.Id;
		this.Ticket = ticket;

		this.state.SwitchState(DynamicAudioStreamState.FadeIn);
	}
	
	public void End()
	{
		this.state.SwitchState(DynamicAudioStreamState.FadeOut);
		
		this.Stop();
	}
	
	// -------------------------------------------------------------------
	// Internal
	// -------------------------------------------------------------------
	internal void SwitchState(DynamicAudioStreamState newState)
	{
		this.state.SwitchState(newState);
	}

	// -------------------------------------------------------------------
	// Private
	// -------------------------------------------------------------------
	private AudioStream GetClip(RuntimeAudioData data, int index)
	{
		return data.ClipKeys[index].LoadManaged<AudioStream>();
	}
}
