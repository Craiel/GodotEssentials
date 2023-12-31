﻿/*namespace Craiel.Essentials.EngineCore;

using System;
using System.Collections.Generic;
using Scene;
using Utils;

public abstract partial class EssentialEngineCore<T, TSceneEnum>
{
    private readonly IDictionary<TSceneEnum, Type> sceneImplementations = new Dictionary<TSceneEnum, Type>();

    private readonly IDictionary<TSceneEnum, BaseScene<TSceneEnum>> scenes = new Dictionary<TSceneEnum, BaseScene<TSceneEnum>>();

    private BaseScene<TSceneEnum> activeScene;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public TSceneEnum? ActiveSceneType { get; private set; }

    public TS GetScene<TS>()
        where TS : BaseScene<TSceneEnum>
    {
        return (TS)this.activeScene;
    }

    protected void RegisterSceneImplementation<TImplementation>(TSceneEnum type)
        where TImplementation : BaseScene<TSceneEnum>
    {
        this.sceneImplementations.Add(type, TypeCache<TImplementation>.Value);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void DestroyScene(TSceneEnum type)
    {
        if (!this.scenes.ContainsKey(type))
        {
            EssentialCore.Logger.Warn($"Scene {type} is not loaded, skipping shutdown");
            return;
        }

        this.scenes.Remove(type);
    }

    private void LoadScene(TSceneEnum type)
    {
        if (!this.scenes.ContainsKey(type))
        {
            BaseScene<TSceneEnum> scene = this.InitializeScene(type);
            if (scene == null)
            {
                return;
            }

            this.scenes.Add(type, scene);
        }
    }

    private BaseScene<TSceneEnum> InitializeScene(TSceneEnum type)
    {
        if (this.scenes.ContainsKey(type))
        {
            EssentialCore.Logger.Warn($"Scene {type} is already loaded, skipping");
            return null;
        }

        Type implementation;
        if (!this.sceneImplementations.TryGetValue(type, out implementation))
        {
            EssentialCore.Logger.Error($"Scene {type} has no implementation defined!");
            return null;
        }

        if (!TypeCache<BaseScene<TSceneEnum>>.Value.IsAssignableFrom(implementation))
        {
            EssentialCore.Logger.Error($"Scene implementation {implementation} is not of type IGameScene!");
            return null;
        }

        return (BaseScene<TSceneEnum>)Activator.CreateInstance(implementation);
    }
}
*/