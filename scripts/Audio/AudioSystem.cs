namespace Craiel.Essentials;

using System.Collections.Generic;
using EngineCore;
using GameData;
using Godot;
using Resource;

public class AudioSystem : IGameModule
{
    private const string DefaultAudioStreamPackedScene = "res://packages/GodotEssentials/data/DynamicAudioStream.tscn";
    
    private static readonly AudioPlayParameters DefaultPlayParameters = new AudioPlayParameters { UseRandomClip = true };
    
    private readonly DynamicAudioStreamPool dynamicAudioStreamPool;
    
    private readonly TicketProviderManaged<AudioTicket, DynamicAudioStream> activeAudio;
    
    private readonly IDictionary<GameDataId, IList<AudioTicket>> sourcesByDataMap;
    
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public AudioSystem()
    {
        this.dynamicAudioStreamPool = new DynamicAudioStreamPool();
        this.sourcesByDataMap = new Dictionary<GameDataId, IList<AudioTicket>>();
        
        this.activeAudio = new TicketProviderManaged<AudioTicket, DynamicAudioStream>();
        this.activeAudio.EnableManagedTickets(this.IsFinished, this.Stop);
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Initialize()
    {
        this.dynamicAudioStreamPool.Initialize(ResourceKey.Create<PackedScene>(DefaultAudioStreamPackedScene), this.UpdateAudioSource);
    }

    public void Update(double delta)
    {
        this.activeAudio.Update(delta);
        
        this.dynamicAudioStreamPool.Update(delta);
    }

    public void Destroy()
    {
    }
    
    public bool IsFinished(AudioTicket ticket)
    {
        if (this.activeAudio.TryGet(ticket, out DynamicAudioStream source))
        {
            return !source.IsActive;
        }

        return true;
    }

    public AudioTicket Play(GameDataId id, AudioPlayParameters parameters = default (AudioPlayParameters))
    {
        var entry = EssentialCore.GameData.Get<RuntimeAudioData>(id);
        if (entry != null)
        {
            DynamicAudioStream source = this.PrepareAudioSource(entry);
            if (source == null)
            {
                return AudioTicket.Invalid;
            }

            var ticket = AudioTicket.Next();
            source.Start(ticket, entry, false, parameters);

            this.RegisterSource(ticket, source);
            return ticket;
        }

        return AudioTicket.Invalid;
    }

    public void Stop(ref AudioTicket ticket)
    {
        if (this.activeAudio.TryGet(ticket, out DynamicAudioStream source))
        {
            source.Stop();
        }

        ticket = AudioTicket.Invalid;
    }

    public void StopByDataId(GameDataId id)
    {
        if (this.sourcesByDataMap.TryGetValue(id, out IList<AudioTicket> tickets))
        {
            for (var i = 0; i < tickets.Count; i++)
            {
                AudioTicket ticket = tickets[i];
                this.Stop(ref ticket);
            }

            tickets.Clear();
            this.sourcesByDataMap.Remove(id);
        }
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private DynamicAudioStream PrepareAudioSource(RuntimeAudioData entry)
    {
        if ((entry.Flags & AudioFlags.Unique) != 0)
        {
            if (this.sourcesByDataMap.ContainsKey(entry.Id))
            {
                // Same audio is already playing and unique
                return null;
            }
        }

        DynamicAudioStream source = this.dynamicAudioStreamPool.Obtain();
        return source;
    }

    private void RegisterSource(AudioTicket ticket, DynamicAudioStream source)
    {
        this.activeAudio.Register(ticket, source);

        IList<AudioTicket> ticketList;
        if (!this.sourcesByDataMap.TryGetValue(source.ActiveId, out ticketList))
        {
            ticketList = new List<AudioTicket>();
            this.sourcesByDataMap.Add(source.ActiveId, ticketList);
        }

        ticketList.Add(ticket);
    }

    private void UnregisterSource(DynamicAudioStream source)
    {
        IList<AudioTicket> ticketList;
        if (this.sourcesByDataMap.TryGetValue(source.ActiveId, out ticketList))
        {
            this.activeAudio.Unregister(source.Ticket);
            
            ticketList.Remove(source.Ticket);
            if (ticketList.Count == 0)
            {
                this.sourcesByDataMap.Remove(source.ActiveId);
            }
        }
    }

    private bool UpdateAudioSource(DynamicAudioStream source)
    {
        if (source.IsActive)
        {
            return true;
        }

        this.UnregisterSource(source);
        return false;
    }
}