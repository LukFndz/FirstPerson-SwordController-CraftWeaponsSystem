using UnityEngine;

namespace FP.Player.Utilities
{
    /// <summary>
    /// Simple and performance-friendly headbob based on immediate input magnitude.
    /// Uses SmoothDamp for responsive fade-out without pops.
    /// </summary>
    public class HeadbobController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _bobAmount = 0.05f;
        [SerializeField] private float _bobFrequency = 12f;
        [SerializeField] private float _returnSmoothTime = 0.08f;
        [SerializeField] private float _moveSmoothTime = 0.04f;

        private Vector3 _initialPos;
        private Vector3 _currentOffset;
        private Vector3 _offsetVelocity;
        private float _timer;

        private void Awake()
        {
            _initialPos = _camera.transform.localPosition;
        }

        public void Tick(float inputMagnitude)
        {
            bool isIntentMoving = inputMagnitude > 0.01f;

            if (isIntentMoving)
            {
                _timer += Time.deltaTime * _bobFrequency * Mathf.Lerp(0.6f, 1.2f, inputMagnitude);

                float bob = Mathf.Sin(_timer) * _bobAmount * inputMagnitude;
                Vector3 target = new Vector3(0f, bob, 0f);

                _currentOffset = Vector3.SmoothDamp(
                    _currentOffset,
                    target,
                    ref _offsetVelocity,
                    _moveSmoothTime
                );
            }
            else
            {
                _currentOffset = Vector3.SmoothDamp(
                    _currentOffset,
                    Vector3.zero,
                    ref _offsetVelocity,
                    _returnSmoothTime
                );
            }

            _camera.transform.localPosition = _initialPos + _currentOffset;
        }
    }
}
