namespace Craiel.Essentials.Geometry;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Extensions;
using Godot;
using Utils;

public class Mesh : IEnumerable<Triangle3Indexed>
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public Mesh()
    {
        this.Vertices = new List<Vector3>();
        this.Triangles = new List<Triangle3Indexed>();
        this.Normals = new List<Vector3>();
        this.NormalMapping = new Dictionary<uint, uint[]>();
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public string Name { get; set; }

    public IList<Vector3> Vertices { get; private set; }

    public IList<Triangle3Indexed> Triangles { get; private set; }

    public IList<Vector3> Normals { get; private set; }

    public IDictionary<uint, uint[]> NormalMapping { get; private set; }

    public Aabb Bounds { get; private set; }

    public IEnumerator<Triangle3Indexed> GetEnumerator()
    {
        return this.Triangles.GetEnumerator();
    }

    public virtual void Clear()
    {
        this.Name = null;
        this.Vertices.Clear();
        this.Triangles.Clear();
        this.Normals.Clear();
        this.Bounds = new Aabb();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.Triangles.GetEnumerator();
    }

    public float[] GetVertexArray()
    {
        float[] result = new float[this.Vertices.Count * 3];
        int index = 0;
        for (var i = 0; i < this.Vertices.Count; i++)
        {
            result[index++] = this.Vertices[i].X;
            result[index++] = this.Vertices[i].Y;
            result[index++] = this.Vertices[i].Z;
        }

        return result;
    }

    public int[] GetTriangleArray()
    {
        int[] result = new int[this.Triangles.Count * 3];
        int index = 0;
        for (var i = 0; i < this.Triangles.Count; i++)
        {
            result[index++] = this.Triangles[i].A;
            result[index++] = this.Triangles[i].B;
            result[index++] = this.Triangles[i].C;
        }

        return result;
    }

    public int[] GetIndexArray()
    {
        int[] result = new int[this.Triangles.Count * 3];

        int index = 0;
        for (var i = 0; i < this.Triangles.Count; i++)
        {
            result[index++] = this.Triangles[i].A;
            result[index++] = this.Triangles[i].B;
            result[index++] = this.Triangles[i].C;
        }

        return result;
    }

    public void Join(Mesh other)
    {
        this.Join(other.Vertices, other.Normals, other.NormalMapping, other.Triangles, Vector3.Zero);
    }

    public void Join(IList<Vector3> vertices, IList<Triangle3Indexed> triangles, Vector3 offset)
    {
        this.Join(vertices, new List<Vector3>(), new Dictionary<uint, uint[]>(), triangles, offset);
    }

    public virtual void Join(
        IList<Vector3> vertices,
        IList<Vector3> normals,
        IDictionary<uint, uint[]> normalMapping,
        IList<Triangle3Indexed> triangles,
        Vector3 offset)
    {
        throw new NotSupportedException("Join not supported in this mesh type");
    }

    public virtual int Split(uint triangleCount, ref IList<Mesh> results)
    {
        if (results == null)
        {
            return 0;
        }

        Mesh currentPart = null;
        for (var i = 0; i < this.Triangles.Count; i++)
        {
            if (currentPart == null)
            {
                currentPart = new Mesh();
            }

            Triangle3Indexed triangle3 = this.Triangles[i];
            Vector3 vertexA = this.Vertices[triangle3.A];
            Vector3 vertexB = this.Vertices[triangle3.B];
            Vector3 vertexC = this.Vertices[triangle3.C];

            int index = currentPart.Vertices.Count;
            currentPart.Vertices.Add(vertexA);
            currentPart.Vertices.Add(vertexB);
            currentPart.Vertices.Add(vertexC);
            currentPart.Triangles.Add(new Triangle3Indexed(index, index + 1, index + 2));

            if (currentPart.Triangles.Count >= triangleCount)
            {
                results.Add(currentPart);
                currentPart = null;
            }
        }

        if (currentPart != null)
        {
            results.Add(currentPart);
        }

        return results.Count;
    }

    public bool Verify()
    {
        bool result = true;
        for (var i = 0; i < this.Triangles.Count; i++)
        {
            var triangle = this.Triangles[i];
            if (triangle.A < 0 || triangle.A >= this.Vertices.Count
                || triangle.B < 0 || triangle.B >= this.Vertices.Count
                || triangle.C < 0 || triangle.C >= this.Vertices.Count)
            {
                EssentialCore.Logger.Error($" - Invalid Triangle {i} / {this.Triangles.Count}: {triangle}");
                result = false;
            }
        }

        return result;
    }

    public void RecalculateBounds()
    {
        this.RecalculateBounds(EssentialMathUtils.Epsilon * 2f);
    }

    // -------------------------------------------------------------------
    // Protected
    // -------------------------------------------------------------------
    protected void RecalculateBounds(float padding)
    {
        var newBounds = new Aabb(VectorExtensions.Fill(-(float.MaxValue / 2f)), VectorExtensions.Fill(float.MaxValue));

        foreach (Triangle3Indexed triangle in this.Triangles)
        {
            var va = this.Vertices[triangle.A];
            var vb = this.Vertices[triangle.B];
            var vc = this.Vertices[triangle.C];
            ApplyVertexToBounds(ref va, ref newBounds);
            ApplyVertexToBounds(ref vb, ref newBounds);
            ApplyVertexToBounds(ref vc, ref newBounds);
        }

        // pad the bounding box a bit to make sure outer triangles are fully contained.
        ApplyPaddingToBounds(padding, ref newBounds);

        this.Bounds = newBounds;
    }

    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "Reviewed. Suppression is OK here.")]
    private static void ApplyVertexToBounds(ref Vector3 vertex, ref Aabb target)
    {
        Vector3 min = target.Position;
        Vector3 max = target.End;

        if (vertex.X < min.X) { min.X = vertex.X; }
        if (vertex.Y < min.Y) { min.Y = vertex.Y; }
        if (vertex.Z < min.Z) { min.Z = vertex.Z; }
        if (vertex.X > max.X) { max.X = vertex.X; }
        if (vertex.Y > max.Y) { max.Y = vertex.Y; }
        if (vertex.Z > max.Z) { max.Z = vertex.Z; }

        target = new Aabb(min, max - min);
    }

    private static void ApplyPaddingToBounds(float padding, ref Aabb target)
    {
        Vector3 paddingVector = VectorExtensions.Fill(padding);
        target.Position += paddingVector;
    }
}
