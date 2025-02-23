using Attributes;
using UnityEngine;
using Utils.MeshGenerator;

namespace Demo
{
    public class RandomMeshesGenerator: MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        
        [Button("Generate New Mesh")] 
        private void GenerateTestMesh()
        {
            if (meshFilter != null)
            {
                meshFilter.mesh = MeshGenerator.GenerateDeformed(6, 1);
            }
            else
            {
                Debug.LogError("MeshFilter not assigned!");
            }
        }
    }
}