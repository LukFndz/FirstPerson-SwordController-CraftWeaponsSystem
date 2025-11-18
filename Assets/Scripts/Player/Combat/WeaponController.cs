using UnityEngine;
using FP.Player.Combat.Attack;
using FP.Player.Combat.Weapon;
using FP.Player.Combat.Hit;
using FP.Input;
using UnityEngine.InputSystem;

namespace FP.Player.Combat
{
    /// <summary>
    /// Controls the currently equipped weapon, handles attack input,
    /// resolves attack direction, plays animations, and enforces cooldown.
    /// </summary>
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponBase _currentWeapon;
        [SerializeField] private HitDetector _hitDetector;
        [SerializeField] private Animator _animator;

        private DirectionalAttackResolver _directionResolver;
        private bool _isAttacking;

        public WeaponBase CurrentWeapon => _currentWeapon;
        public bool IsAttacking => _isAttacking;

        private void Awake()
        {
            _directionResolver = new DirectionalAttackResolver();
        }

        public void Tick()
        {
            HandleAttack();

            if (_isAttacking) return;

            var look = InputManager.Instance.GetVector2Value("Look");
            UpdateMouseDelta(look);
            HandleIdlePosture();
            HandleAttackIndicatorUI();
        }

        /// <summary>
        /// Should be called externally with the latest mouse delta each frame.
        /// </summary>
        public void UpdateMouseDelta(Vector2 mouseDelta)
        {
            if(mouseDelta == Vector2.zero) return;

            _directionResolver.UpdateMouseDelta(mouseDelta);
        }

        private AttackDirection _lastDirection = AttackDirection.None;
        private float _lastIdleTarget;

        private void HandleIdlePosture()
        {
            var direction = _directionResolver.ResolveDirection();

            // If direction is None, keep the last valid one
            if (direction == AttackDirection.None)
                direction = _lastDirection;
            else
                _lastDirection = direction;

            float target = direction switch
            {
                AttackDirection.Left => 0f,
                AttackDirection.Up => 0.5f,
                AttackDirection.Right => 1f,
                _ => _lastIdleTarget
            };

            _lastIdleTarget = target;

            _animator.SetFloat("Idle", target, 0.1f, Time.deltaTime);
        }

        private void HandleAttackIndicatorUI()
        {
            UIManager.Instance.AttackIndicatorUI.SetDirection(_lastDirection);
        }

        private void HandleAttack()
        {
            if (InputManager.Instance.IsActionTriggered("Attack"))
                TryAttack();
        }

        /// <summary>
        /// Triggers an attack with the currently equipped weapon.
        /// Handles cooldown and calls the weapon's attack logic.
        /// </summary>
        public void TryAttack()
        {
            if (_currentWeapon == null)
                return;

            if (_isAttacking)
                return;

            AttackDirection direction = _directionResolver.ResolveDirection();

            if (direction == AttackDirection.None)
                direction = _lastDirection;
            else
                _lastDirection = direction;

            _isAttacking = true;

            int attackIndex = direction switch
            {
                AttackDirection.Left => 0,
                AttackDirection.Up => 1,
                AttackDirection.Right => 2,
                _ => 1
            };

            _animator.SetInteger("AttackIndex", attackIndex);
            _animator.SetBool("IsAttacking", true);
        }


        public void AttackFrame()
        {
            _animator.SetBool("IsAttacking", false);
            _currentWeapon.PerformAttack(_lastDirection, _hitDetector);
        }

        private void ResetAttackState()
        {
            _isAttacking = false;
        }

        /// <summary>
        /// Equips a new weapon at runtime.
        /// </summary>
        public void EquipWeapon(WeaponBase weapon)
        {
            _currentWeapon = weapon;
        }
    }
}
