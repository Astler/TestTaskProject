using UnityEngine;

namespace Utils.MeshGenerator
{
    public struct GeneratedMeshData
    {
        public Mesh Mesh { get; private set; }
        public Vector3[] Vertices { get; }
        public int[] Triangles { get; }

        public GeneratedMeshData(Vector3[] vertices, int[] triangles) : this()
        {
            Vertices = vertices;
            Triangles = triangles;
            RecalculateMesh();
        }

        public GeneratedMeshData RecalculateMesh()
        {
            Mesh = new Mesh {vertices = Vertices, triangles = Triangles};
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            return this;
        }
    }
}