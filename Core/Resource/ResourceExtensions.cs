namespace Craiel.Essentials.Runtime.Resource;

using Godot;

public static class ResourceExtensions
{
    public static T LoadManaged<T>(this ResourceKey key)
        where T : Resource
    {
        using (var resource = ResourceProvider.Instance.AcquireOrLoadResource<T>(key))
        {
            if (resource == null)
            {
                return default;
            }

            return resource.Data;
        }
    }
}
