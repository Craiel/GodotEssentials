namespace Craiel.Essentials.SaveLoad;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class PersistentFieldAttribute : System.Attribute
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public PersistentFieldAttribute(string key)
    {
        Key = key;
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public string Key { get; }
}