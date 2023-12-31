namespace Craiel.Essentials.Geometry;

using System;
using System.Collections.Generic;
using Godot;

public class StaticMesh : Mesh
{
    public bool HasGeometry { get; private set; }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Clear()
    {
        base.Clear();
        this.HasGeometry = false;
    }

    public override void Join(IList<Vector3> vertices, IList<Vector3> normals, IDictionary<uint, uint[]> normalMapping, IList<Triangle3Indexed> triangles, Vector3 offset)
    {
        if (this.HasGeometry)
        {
            throw new InvalidOperationException("Join attempted on Static Mesh with geometry data, call clear first before setting new data!");
        }

        if (offset == Vector3.Zero)
        {
            Extensions.CollectionExtensions.AddRange(this.Vertices, vertices);
        }
        else
        {
            foreach (Vector3 vertex in vertices)
            {
                this.Vertices.Add(vertex + offset);
            }
        }
        
        Extensions.CollectionExtensions.AddRange(this.Normals, normals);
        Extensions.CollectionExtensions.AddRange(this.NormalMapping, normalMapping);
        Extensions.CollectionExtensions.AddRange(this.Triangles, triangles);

        this.RecalculateBounds();
    }
}