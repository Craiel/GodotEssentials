namespace Craiel.Essentials.Runtime.Geometry;

using System.IO;
using Godot;

public static class ObjExport
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------

    // see https://en.wikipedia.org/wiki/Wavefront_.obj_file
    public static void Export(Mesh mesh, StreamWriter target)
    {
        EssentialsCore.Logger.Info("Saving to stream");

        int lineCount = 4;

        target.WriteLine(string.Format("g {0}", mesh.Name ?? "No Name"));

        EssentialsCore.Logger.Info($"  - {mesh.Vertices.Count} vertices");
        foreach (Vector3 vertex in mesh.Vertices)
        {
            target.WriteLine(string.Format("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z));
            lineCount++;
        }

        target.WriteLine();

        EssentialsCore.Logger.Info($"  - {mesh.Normals.Count} normals");
        foreach (Vector3 normal in mesh.Normals)
        {
            target.WriteLine(string.Format("vn {0} {1} {2}", normal.X, normal.Y, normal.Z));
            lineCount++;
        }

        target.WriteLine();

        EssentialsCore.Logger.Info($"  - {mesh.Triangles.Count} triangles");
        for (var i = 0; i < mesh.Triangles.Count; i++)
        {
            var triangle = mesh.Triangles[i];

            // Currently we do not support texture coordinates
            if (mesh.Normals.Count > 0)
            {
                target.WriteLine(string.Format("f {0}//{1} {2}//{3} {4}//{5}", triangle.A + 1,
                    mesh.NormalMapping[(uint) i][0] + 1, triangle.B + 1, mesh.NormalMapping[(uint) i][1] + 1,
                    triangle.C + 1, mesh.NormalMapping[(uint) i][2] + 1));
            }
            else
            {
                target.WriteLine($"f {triangle.A + 1} {triangle.B + 1} {triangle.C + 1}");
            }

            lineCount++;
        }

        EssentialsCore.Logger.Info($"  {lineCount} lines");
    }
}
