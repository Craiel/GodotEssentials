namespace Craiel.Essentials.DB;

using System;
using System.Collections.Generic;
using System.Reflection;

public static class GameDBUtils
{
    private const string DBClearMethod = "Clear";
    
    private static bool rescanRequired = true;
    private static IList<MethodInfo> databaseClears = new List<MethodInfo>();
    private static IList<MethodInfo> entryRegistrations = new List<MethodInfo>();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void ReloadGameData(bool forceRescan = false)
    {
        if (rescanRequired || forceRescan)
        {
            RescanDB();
        }

        foreach (MethodInfo clear in databaseClears)
        {
            clear.Invoke(null, null);
        }

        foreach (MethodInfo registration in entryRegistrations)
        {
            registration.Invoke(null, null);
        }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private static void RescanDB()
    {
        databaseClears.Clear();
        entryRegistrations.Clear();
        
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
        {
            foreach (Type type in assembly.GetTypes())
            {
                var dbAttribute = type.GetCustomAttribute<GameDatabaseAttribute>(false);
                if (dbAttribute != null)
                {
                    var method = type.GetMethod(DBClearMethod, BindingFlags.Public | BindingFlags.Static);
                    if (method == null)
                    {
                        throw new InvalidOperationException("Database is missing reload method: " + type);
                    }
                    
                    databaseClears.Add(method);
                    continue;
                }
                
                foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var attribs = methodInfo.GetCustomAttribute<GameDataDefinitionAttribute>(false);
                    if (attribs != null)
                    {
                        entryRegistrations.Add(methodInfo);
                    }
                }
            }
        }

        rescanRequired = false;
    }
}