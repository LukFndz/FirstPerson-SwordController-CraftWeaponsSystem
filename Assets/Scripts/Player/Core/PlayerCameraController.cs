using FP.Input;
using UnityEngine;

namespace FP.Player.Core
{
    /// <summary>
    /// Handles camera rotation based on look input.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private float _sensitivity = 2f;
        [SerializeField] private float _verticalClamp = 80f;

        private float _xRotation;

        /// <summary>
        /// Rotates the camera root and player horizontally.
        /// </summary>
        public void Tick()
        {
            var look = InputManager.Instance.GetVector2Value("Look");

            float mouseX = look.x * _sensitivity;
            float mouseY = look.y * _sensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -_verticalClamp, _verticalClamp);

            _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
