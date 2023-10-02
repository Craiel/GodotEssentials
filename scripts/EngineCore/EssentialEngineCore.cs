/*namespace Craiel.Essentials.EngineCore;

using System;
using System.Collections.Generic;
using System.Linq;
using Collections;
using Event;
using Events;
using I18N;
using Modules;
using Resource;
using Threading;
using TweenLite;

public static class EssentialEngineCore
{
    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected virtual void LoadPermanentResources()
    {
        // Todo: Add resources we will always need

        ResourceProvider.Instance.LoadImmediate();
    }

    protected abstract void InitializeEngineComponents();
    
    protected void SetGameModules(bool includeDefaultModules = true, params IGameModule[] newModules)
    {
        using (TempList<IGameModule> moduleList = new TempList<IGameModule>())
        {
            if (includeDefaultModules)
            {
                moduleList.Add(this.SaveLoad);
            }

            if (newModules == null || newModules.Length == 0)
            {
                this.SetGameModules(moduleList.List);
                return;
            }

            foreach (IGameModule module in newModules)
            {
                if (moduleList.Contains(module))
                {
                    continue;
                }

                moduleList.Add(module);
            }

            this.SetGameModules(moduleList.List);
        }
    }
    
    private void SetGameModules(IList<IGameModule> moduleList)
    {
        if (moduleList == null)
        {
            this.gameModules = Array.Empty<IGameModule>();
            return;
        }
        
        this.gameModules = moduleList.ToArray();
    }
}*/