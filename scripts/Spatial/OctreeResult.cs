namespace Craiel.Essentials.Spatial;

using Godot;

public struct OctreeResult<T>
    where T : class
{
    public readonly T Entry;

    public Vector3 Position;

    public OctreeResult(T entry, Vector3 position)
    {
        this.Entry = entry;
        this.Position = position;
    }
}
