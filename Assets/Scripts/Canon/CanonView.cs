using System.Collections;
using UnityEngine;

namespace Canon
{
    public class CanonView : MonoBehaviour
    {
        [SerializeField] private Transform horizontalRoot;
        [SerializeField] private Transform verticalRoot;

        [field: SerializeField] public Transform ShotPosition { get; private set; }
        [field: SerializeField] public TrajectoryRenderer TrajectoryRenderer { get; private set; }

        [Header("Input Settings"), SerializeField]
        private float horizontalSpeed = 2f;

        [SerializeField] private float verticalSpeed = 2f;
        [SerializeField] private float minVerticalAngle = -20f;
        [SerializeField] private float maxVerticalAngle = 60f;

        [Header("Recoil Settings"), SerializeField]
        private float recoilDuration = 0.1f;

        [SerializeField] private float returnDuration = 0.2f;

        private Vector3 _originalCanonPosition;
        private Coroutine _recoilCoroutine;

        private void Awake()
        {
            _originalCanonPosition = verticalRoot.localPosition;
        }

        public void PlayRecoilAnimation(float recoilDistance)
        {
            if (_recoilCoroutine != null)
            {
                StopCoroutine(_recoilCoroutine);
            }

            _recoilCoroutine = StartCoroutine(RecoilCoroutine(recoilDistance));
        }

        private IEnumerator RecoilCoroutine(float recoilDistance)
        {
            float elapsed = 0f;
            Vector3 recoilTarget = _originalCanonPosition - new Vector3(0f, 0f, 1f) * recoilDistance;

            while (elapsed < recoilDuration)
            {
                elapsed += Time.deltaTime;
                verticalRoot.localPosition =
                    Vector3.Lerp(_originalCanonPosition, recoilTarget, elapsed / recoilDuration);
                yield return null;
            }

            elapsed = 0f;
            Vector3 currentPos = verticalRoot.localPosition;

            while (elapsed < returnDuration)
            {
                elapsed += Time.deltaTime;
                verticalRoot.localPosition = Vector3.Lerp(currentPos, _originalCanonPosition, elapsed / returnDuration);
                yield return null;
            }

            verticalRoot.localPosition = _originalCanonPosition;
            _recoilCoroutine = null;
        }

        public void UpdateAim(Vector2 aimDelta)
        {
            float horizontalRotation = aimDelta.x * horizontalSpeed * Time.deltaTime;
            horizontalRoot.Rotate(Vector3.up, horizontalRotation);

            float verticalRotation = -aimDelta.y * verticalSpeed * Time.deltaTime;

            float currentAngle = verticalRoot.localEulerAngles.x;
            if (currentAngle > 180) currentAngle -= 360f;

            float newAngle = Mathf.Clamp(currentAngle + verticalRotation, minVerticalAngle, maxVerticalAngle);
            verticalRoot.localEulerAngles = new Vector3(newAngle, 0, 0);
        }
    }
}