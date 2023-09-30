namespace Craiel.Essentials.Runtime.Extensions;

using Godot;

public static class RayCastExtensions
{
    public static Vector3 Direction(this RayCast3D ray)
    {
        return ray.Position.DirectionTo(ray.TargetPosition);
    }
}