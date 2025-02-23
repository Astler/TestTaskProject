using UnityEngine;
using System.Collections;

namespace GameCamera
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _initialPosition;
        private Coroutine _shakeCoroutine;
        private float _seed;

        private void Awake()
        {
            _initialPosition = transform.localPosition;
            _seed = Random.value * 100f;
        }

        public void RequestShake(float duration = 0.2f, float intensity = 0.05f)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, intensity));
        }

        private IEnumerator ShakeCoroutine(float duration, float intensity)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float progress = elapsed / duration;
                float damping = 1f - progress;

                float x = (Mathf.PerlinNoise(_seed + elapsed * 10f, 0f) * 2f - 1f) * intensity * damping;
                float y = (Mathf.PerlinNoise(0f, _seed + elapsed * 10f) * 2f - 1f) * intensity * damping;

                transform.localPosition = _initialPosition + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _initialPosition;
            _shakeCoroutine = null;
        }
    }
}