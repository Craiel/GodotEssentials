#if DEBUG
namespace Craiel.Essentials.DebugTools;

using System;
using System.Collections.Generic;
using System.Reflection;

public static class IntegrityTestRunner
{
    private const string RunMethodName = "Run";
    
    private static readonly IList<MethodInfo> Runners = new List<MethodInfo>();
    
    private static bool rescanRequired = true;

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void RunAll(bool forceRescan = false)
    {
        if (rescanRequired || forceRescan)
        {
            RescanRunners();
        }

        foreach (MethodInfo run in Runners)
        {
            run.Invoke(null, null);
        }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void RescanRunners()
    {
        Runners.Clear();
        
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
        {
            foreach (Type type in assembly.GetTypes())
            {
                var dbAttribute = type.GetCustomAttribute<IntegrityTestAttribute>(false);
                if (dbAttribute != null)
                {
                    var method = type.GetMethod(RunMethodName, BindingFlags.Public | BindingFlags.Static);
                    if (method == null)
                    {
                        throw new InvalidOperationException("Runner is missing method: " + type);
                    }
                    
                    Runners.Add(method);
                }
            }
        }

        rescanRequired = false;
    }
}
#endif