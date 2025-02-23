using UnityEngine;

namespace Utils.MeshGenerator
{
    public static class MeshGenerator
    {
        public static Mesh GenerateDeformed(int segments, float radius)
        {
            GeneratedMeshData data = GenerateCircleMesh(segments, radius);
            return DeformMesh(data).Mesh;
        }

        private static GeneratedMeshData GenerateCircleMesh(int segments, float radius)
        {
            int vertexCount = (segments + 1) * (segments + 1);
            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];

            for (int lat = 0; lat <= segments; lat++)
            {
                float phi = Mathf.PI * lat / segments;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                for (int lon = 0; lon <= segments; lon++)
                {
                    float theta = 2f * Mathf.PI * lon / segments;
                    int index = lat * (segments + 1) + lon;

                    vertices[index] = new Vector3(radius * sinPhi * Mathf.Cos(theta), radius * cosPhi,
                        radius * sinPhi * Mathf.Sin(theta));

                    uv[index] = new Vector2((float) lon / segments, (float) lat / segments);
                }
            }

            int[] triangles = new int[segments * segments * 6];
            int triangleIndex = 0;

            for (int lat = 0; lat < segments; lat++)
            {
                for (int lon = 0; lon < segments; lon++)
                {
                    int current = lat * (segments + 1) + lon;
                    int next = current + segments + 1;

                    triangles[triangleIndex++] = current;
                    triangles[triangleIndex++] = current + 1;
                    triangles[triangleIndex++] = next;

                    triangles[triangleIndex++] = next;
                    triangles[triangleIndex++] = current + 1;
                    triangles[triangleIndex++] = next + 1;
                }
            }

            return new GeneratedMeshData(vertices, triangles);
        }

        private static GeneratedMeshData DeformMesh(GeneratedMeshData meshData, float noiseScale = 1f,
            float deformStrength = 0.3f)
        {
            float randomOffsetX = Random.Range(0f, 100f);
            float randomOffsetY = Random.Range(0f, 100f);

            for (int i = 0; i < meshData.Vertices.Length; i++)
            {
                Vector3 vertex = meshData.Vertices[i];

                float noise = Mathf.PerlinNoise(vertex.x + randomOffsetX, vertex.y + randomOffsetY);
                float deformation = 1f + (noise - 0.5f) * deformStrength;
                meshData.Vertices[i] *= deformation;
            }

            return meshData.RecalculateMesh();
        }
    }
}