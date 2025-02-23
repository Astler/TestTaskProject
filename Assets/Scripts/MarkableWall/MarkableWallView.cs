using UnityEngine;

namespace MarkableWall
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MarkableWallView : MonoBehaviour
    {
        private static readonly int MarksTexture = Shader.PropertyToID("_MarksTexture");
        [SerializeField] private int textureSize = 1024;
        [SerializeField] private Texture2D markTexture;
        [SerializeField] private float markSize = 0.1f;

        private RenderTexture _marksTexture;
        private Material _instancedMaterial;
        private float _wallSize;

        private void Awake()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            _marksTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32)
            {
                enableRandomWrite = true
            };

            _instancedMaterial = new Material(meshRenderer.material);
            _instancedMaterial.SetTexture(MarksTexture, _marksTexture);
            meshRenderer.material = _instancedMaterial;

            _wallSize = 10f;
        }

        public void AddMark(RaycastHit hit)
        {
            if (hit.collider.gameObject != gameObject)
            {
                Debug.Log("Hit object is not the target wall.");
                return;
            }

            Vector3 hitPoint = hit.point;
            Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint);

            float uvX = (localHitPoint.x / _wallSize) + 0.5f;
            float uvY = (localHitPoint.z / _wallSize) + 0.5f;
            Vector2 uv = new(uvX, uvY);

            float size = markSize * textureSize;
            float x = (1 - uv.x) * textureSize - size * 0.5f;
            float y = uv.y * textureSize - size * 0.5f;
            Rect markRect = new(x, y, size, size);

            RenderTexture prevRT = RenderTexture.active;
            RenderTexture.active = _marksTexture;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, textureSize, textureSize, 0);

            Graphics.DrawTexture(markRect, markTexture);

            GL.PopMatrix();
            RenderTexture.active = prevRT;
        }

        private void OnDestroy()
        {
            if (_marksTexture)
            {
                _marksTexture.Release();
            }

            if (_instancedMaterial)
            {
                Destroy(_instancedMaterial);
            }
        }
    }
}