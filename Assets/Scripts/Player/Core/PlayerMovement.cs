using UnityEngine;
using FP.Input;

namespace FP.Player.Core
{
    /// <summary>
    /// Handles character movement, gravity and acceleration.
    /// Exposes immediate input magnitude so other systems (headbob) can react instantly.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 4f;
        [SerializeField] private float _sprintSpeed = 7f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _gravity = -20f;

        private CharacterController _controller;
        private Vector3 _velocity;
        private float _currentSpeed;

        public float InputMagnitude { get; private set; }

        public float CurrentSpeed => _currentSpeed;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Main movement loop called from PlayerController.
        /// </summary>
        public void Tick()
        {
            HandleMovement();
            ApplyGravity();
        }

        private void HandleMovement()
        {
            var input = InputManager.Instance.GetVector2Value("Move");

            // Immediate raw magnitude from input (no smoothing).
            InputMagnitude = Mathf.Clamp01(new Vector2(input.x, input.y).magnitude);

            var direction = new Vector3(input.x, 0f, input.y);
            direction = transform.TransformDirection(direction);

            var targetSpeed = InputManager.Instance.IsActionPressed("Sprint")
                ? _sprintSpeed
                : _walkSpeed;

            // Smooth current speed for physics/movement, but do not use it for headbob.
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed * direction.magnitude, _acceleration * Time.deltaTime);

            var move = direction.normalized * _currentSpeed;
            _controller.Move(move * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (_controller.isGrounded && _velocity.y < 0f)
                _velocity.y = -2f;

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
