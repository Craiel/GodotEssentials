using Craiel.Essentials.Contracts;

namespace Craiel.Essentials.GameData;

using System;
using System.Collections.Generic;
using System.IO;
using Event;
using Events;
using Resource;
using Utils;

public class GameRuntimeData : IGameModule
{
    private static readonly IList<Type> DataRegister = new List<Type>();

    private readonly GameDataReader reader;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public GameRuntimeData()
    {
        this.reader = InitializeReader();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public bool IsLoaded { get; private set; }

    public void Initialize()
    {
    }

    public void Update(double delta)
    {
    }

    public void Destroy()
    {
    }

    public bool GetAll<T>(IList<T> target)
    {
        return this.reader.GetAll(target);
    }

    public T Get<T>(GameDataId dataId)
    {
        return this.reader.Get<T>(dataId);
    }

    public GameDataId GetRuntimeId(GameDataRefBase refData)
    {
        if (refData == null || !refData.IsValid())
        {
            return GameDataId.Invalid;
        }

        return new GameDataId(refData.Id);
    }

    public void Load(ResourceKey resourceKey)
    {
        var asset = resourceKey.LoadManaged<Godot.Resource>();
        if (asset == null)
        {
            EssentialCore.Logger.Error($"Could not load RuntimeData from resource {resourceKey}");
            return;
        }

        throw new NotImplementedException("Runtime GameData Load Incomplete");
        //this.Load(asset.bytes);
    }

    public void Load(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        {
            this.reader.Load(stream);
        }

        this.IsLoaded = true;

        GameEvents.Send(new EventGameDataLoaded());
    }

    public static void RegisterData<T>()
        where T : RuntimeGameData
    {
        DataRegister.Add(TypeDef<T>.Value);
    }

    public static GameDataReader InitializeReader()
    {
        var reader = new GameDataReader();

        foreach (Type dataType in DataRegister)
        {
            reader.RegisterData(dataType);
        }

        return reader;
    }
}