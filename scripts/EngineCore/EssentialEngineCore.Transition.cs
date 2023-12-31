﻿/*namespace Craiel.Essentials.EngineCore;

using System;
using System.Collections.Generic;
using Enums;
using Event;
using Events;

public abstract partial class EssentialEngineCore<T, TSceneEnum>
{
    private bool transitioning;
    private TSceneEnum transitionTarget;
    private SceneTransitionStep transitionStep;
    private object[] transitionData;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public delegate void TransitionStartingDelegate(TSceneEnum? current, TSceneEnum target);
    public delegate void TransitionFinishedDelegate(TSceneEnum current);

    public event TransitionStartingDelegate TransitionStarting;
    public event TransitionFinishedDelegate TransitionFinished;

    public bool InTransition { get; private set; }

    public void Transition(TSceneEnum type, params object[] data)
    {
        this.BeginTransition(type, data);
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private void BeginTransition(TSceneEnum type, params object[] data)
    {
        this.transitioning = true;
        this.transitionTarget = type;
        this.transitionStep = SceneTransitionStep.Initialize;
        this.transitionData = data;

        this.InTransition = true;

        if (this.ActiveSceneType.HasValue 
            && EqualityComparer<TSceneEnum>.Default.Equals(this.ActiveSceneType.Value, type))
        {
            EssentialCore.Logger.Warn("Transition target and active scene are the same, skipping!");
            return;
        }

        EssentialCore.Logger.Info($"Transitioning to {type}");
        
        GameEvents.Send(new EventSceneTransitionStarting(type));

        if (this.TransitionStarting != null)
        {
            this.TransitionStarting(this.ActiveSceneType, type);
        }

        this.LoadScene(type);
    }

    private void UpdateSceneTransition()
    {
        // Check if we are still on the previous scene
        if (this.activeScene != null 
            && !EqualityComparer<TSceneEnum>.Default.Equals(this.activeScene.Type, this.transitionTarget))
        {
            // We need to destroy this scene
            if (this.activeScene.ContinueDestroy(this.transitionStep))
            {
                // The scene is still processing the destroy step
                return;
            }

            if (this.AdvanceDestroyTransition())
            {
                // Transitioned to the next step
                return;
            }

            // We are done destroying the scene
            this.DestroyScene(this.activeScene.Type);
            this.activeScene = null;
            this.ActiveSceneType = null;
            this.transitionStep = SceneTransitionStep.Initialize;

            // Destroy done, moving to load phase
        }

        // Check if the scene is not active yet
        if (this.activeScene == null)
        {
            this.LoadScene(this.transitionTarget);
            this.activeScene = this.scenes[this.transitionTarget];
            this.activeScene.SetData(this.transitionData);

            EssentialCore.Logger.Info($"Activated target scene {this.transitionTarget}");
        }

        if (this.activeScene.ContinueLoad(this.transitionStep))
        {
            // Still processing load step
            return;
        }

        if (this.AdvanceLoadTransition())
        {
            return;
        }

        // We are done transitioning
        EssentialCore.Logger.Info($"Transition to {this.transitionTarget} completed");

        this.transitioning = false;
        this.ActiveSceneType = this.activeScene.Type;

        this.InTransition = false;
        
        GameEvents.Send(new EventSceneTransitionFinished(this.ActiveSceneType));

        if (this.TransitionFinished != null)
        {
            this.TransitionFinished(this.ActiveSceneType.Value);
        }
    }

    private bool AdvanceDestroyTransition()
    {
        switch (this.transitionStep)
        {
            case SceneTransitionStep.Initialize:
                {
                    this.transitionStep = SceneTransitionStep.PreDestroy;
                    return true;
                }

            case SceneTransitionStep.PreDestroy:
                {
                    this.transitionStep = SceneTransitionStep.Destroy;
                    return true;
                }

            case SceneTransitionStep.Destroy:
                {
                    this.transitionStep = SceneTransitionStep.PostDestroy;
                    return true;
                }

            case SceneTransitionStep.PostDestroy:
                {
                    this.transitionStep = SceneTransitionStep.Finalize;
                    return true;
                }

            case SceneTransitionStep.Finalize:
                {
                    return false;
                }

            default:
                {
                    throw new InvalidOperationException("Invalid state: " + this.transitionStep);
                }
        }
    }

    private bool AdvanceLoadTransition()
    {
        switch (this.transitionStep)
        {
            case SceneTransitionStep.Initialize:
                {
                    this.transitionStep = SceneTransitionStep.PreLoad;
                    return true;
                }

            case SceneTransitionStep.PreLoad:
                {
                    this.transitionStep = SceneTransitionStep.LoadRegisterResources1;
                    return true;
                }

            case SceneTransitionStep.LoadRegisterResources1:
                {
                    this.transitionStep = SceneTransitionStep.LoadResources1;
                    return true;
                }

            case SceneTransitionStep.LoadResources1:
                {
                    this.transitionStep = SceneTransitionStep.LoadRegisterResources2;
                    return true;
                }

            case SceneTransitionStep.LoadRegisterResources2:
                {
                    this.transitionStep = SceneTransitionStep.LoadResources2;
                    return true;
                }

            case SceneTransitionStep.LoadResources2:
                {
                    this.transitionStep = SceneTransitionStep.LoadRegisterResources3;
                    return true;
                }

            case SceneTransitionStep.LoadRegisterResources3:
                {
                    this.transitionStep = SceneTransitionStep.LoadResources3;
                    return true;
                }

            case SceneTransitionStep.LoadResources3:
                {
                    this.transitionStep = SceneTransitionStep.Load;
                    return true;
                }

            case SceneTransitionStep.Load:
                {
                    this.transitionStep = SceneTransitionStep.PostLoad;
                    return true;
                }

            case SceneTransitionStep.PostLoad:
                {
                    this.transitionStep = SceneTransitionStep.Finalize;
                    return true;
                }

            case SceneTransitionStep.Finalize:
                {
                    return false;
                }

            default:
                {
                    throw new InvalidOperationException("Invalid state: " + this.transitionStep);
                }
        }
    }
}
*/