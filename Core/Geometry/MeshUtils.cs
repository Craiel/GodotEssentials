namespace Craiel.Essentials.Runtime.Geometry;

using System;
using System.Collections.Generic;
using Extensions;
using Godot;
using Spatial;
using Utils;

public static class MeshUtils
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static void BuildMinCoordinate(ref Vector3 target, Vector3 coordinate)
    {
        if (coordinate.X < target.X) { target.X = coordinate.X; }
        if (coordinate.Y < target.Y) { target.Y = coordinate.Y; }
        if (coordinate.Z < target.Z) { target.Z = coordinate.Z; }
    }

    public static void BuildMaxCoordinate(ref Vector3 target, Vector3 coordinate)
    {
        if (coordinate.X > target.X) { target.X = coordinate.X; }
        if (coordinate.Y > target.Y) { target.Y = coordinate.Y; }
        if (coordinate.Z > target.Z) { target.Z = coordinate.Z; }
    }

    public static void CleanOrphanVertices(IList<Vector3> vertices, IList<Triangle3Indexed> triangles, out IList<Vector3> cleanVertices, out IList<Triangle3Indexed> cleanTriangles)
    {
        Vector3 minCoordinate = VectorExtensions.Fill(float.MaxValue);
        Vector3 maxCoordinate = VectorExtensions.Fill(float.MinValue);

        // First we build non-indexed triangles so we can re-index them after the cleanup
        IList<Triangle3> triangleList = new List<Triangle3>();
        for (var i = 0; i < triangles.Count; i++)
        {
            Triangle3Indexed indexed = triangles[i];
            if (!IsVertexValid(vertices[indexed.A]) || !IsVertexValid(vertices[indexed.B]) || !IsVertexValid(vertices[indexed.C]))
            {
                // This triangle has invalid vertices, ignore
                EssentialCore.Logger.Warn("- Triangle Vertex out of safe range, skipping!");
                continue;
            }

            Triangle3 nonIndexed = new Triangle3(vertices[indexed.A], vertices[indexed.B], vertices[indexed.C]);
            triangleList.Add(nonIndexed);

            BuildMinCoordinate(ref minCoordinate, nonIndexed.A);
            BuildMinCoordinate(ref minCoordinate, nonIndexed.B);
            BuildMinCoordinate(ref minCoordinate, nonIndexed.C);

            BuildMaxCoordinate(ref maxCoordinate, nonIndexed.A);
            BuildMaxCoordinate(ref maxCoordinate, nonIndexed.B);
            BuildMaxCoordinate(ref maxCoordinate, nonIndexed.C);
        }

        cleanVertices = new List<Vector3>();
        cleanTriangles = new List<Triangle3Indexed>();

        //float size = Math.Abs((maxCoordinate - minCoordinate).Length());
        Octree<MeshSpatialInfo> cleanTree = new Octree<MeshSpatialInfo>(1, minCoordinate, 1);

        for (var i = 0; i < triangleList.Count; i++)
        {
            Triangle3 triangle = triangleList[i];
            uint indexA;
            uint indexB;
            uint indexC;
            
            if (!IndexOfVertex(cleanTree, triangle.A, out indexA))
            {
                indexA = AddNewVertex(cleanVertices, triangle.A, cleanTree);
            }
            
            if (!IndexOfVertex(cleanTree, triangle.B, out indexB))
            {
                indexB = AddNewVertex(cleanVertices, triangle.B, cleanTree);
            }
            
            if (!IndexOfVertex(cleanTree, triangle.C, out indexC))
            {
                indexC = AddNewVertex(cleanVertices, triangle.C, cleanTree);
            }

            cleanTriangles.Add(new Triangle3Indexed(indexA, indexB, indexC));
        }

        if (cleanVertices.Count != vertices.Count)
        {
            EssentialCore.Logger.Info($"- {vertices.Count - cleanVertices.Count} orphan vertices");
        }
    }

    // -------------------------------------------------------------------
    // Internal
    // -------------------------------------------------------------------
    internal static bool IsVertexValid(Vector3 vertex)
    {
        return Math.Abs(vertex.Length()) < EssentialMathUtils.MaxFloat;
    }

    internal static uint AddNewVertex(IList<Vector3> target, Vector3 vertex, Octree<MeshSpatialInfo> mergeTree)
    {
        target.Add(vertex);
        uint index = (uint)target.Count - 1;

        if (mergeTree != null)
        {
            OctreeResult<MeshSpatialInfo> info;
            if (mergeTree.GetAt(vertex, out info))
            {
                info.Entry.Vertex = index;
            }
            else
            {
                mergeTree.Add(new MeshSpatialInfo(index), vertex);
            }
        }

        return index;
    }

    internal static uint AddNewNormal(IList<Vector3> target, Vector3 normal, Octree<MeshSpatialInfo> mergeTree)
    {
        target.Add(normal);
        uint index = (uint)target.Count - 1;

        if (mergeTree != null)
        {
            OctreeResult<MeshSpatialInfo> info;
            if (mergeTree.GetAt(normal, out info))
            {
                info.Entry.Normal = index;
            }
            else
            {
                mergeTree.Add(new MeshSpatialInfo(null, index), normal);
            }
        }

        return index;
    }

    internal static bool IndexOfVertex(Octree<MeshSpatialInfo> tree, Vector3 position, out uint index)
    {
        OctreeResult<MeshSpatialInfo> result;
        if (tree.GetAt(position, out result) && result.Entry.Vertex != null)
        {
            index = result.Entry.Vertex.Value;
            return true;
        }

        index = 0;
        return false;
    }

    internal static bool IndexOfNormal(Octree<MeshSpatialInfo> tree, Vector3 position, out uint index)
    {
        OctreeResult<MeshSpatialInfo> result;
        if (tree.GetAt(position, out result) && result.Entry.Normal != null)
        {
            index = result.Entry.Normal.Value;
            return true;
        }

        index = 0;
        return false;
    }
}