using UnityEngine;
using FP.Player.Combat.Attack;
using FP.Player.Combat.Weapon;
using FP.Player.Combat.Hit;
using FP.Input;

namespace FP.Player.Combat
{
    /// <summary>
    /// Controller responsible for managing the player's weapon, handling attacks, and coordinating animations.
    /// </summary>
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponBase _currentWeapon;
        [SerializeField] private HitDetector _hitDetector;
        [SerializeField] private Animator _animator;

        private DirectionalAttackResolver _directionResolver;
        private bool _isAttacking;
        private AttackDirection _lastDirection = AttackDirection.None;
        private float _lastIdleTarget;

        public WeaponBase CurrentWeapon => _currentWeapon;
        public bool IsAttacking => _isAttacking;

        private void Awake()
        {
            _directionResolver = new DirectionalAttackResolver();
            if (_currentWeapon == null) return;
            _currentWeapon.Initialize(this, _hitDetector);
        }

        public void Tick()
        {
            HandleAttack();

            if (_isAttacking)
                return;

            var look = InputManager.Instance.GetVector2Value("Look");
            UpdateMouseDelta(look);

            HandleIdlePosture();
            HandleAttackIndicatorUI();
        }

        public void UpdateMouseDelta(Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.zero)
                _directionResolver.UpdateMouseDelta(mouseDelta);
        }

        private void HandleIdlePosture()
        {
            var dir = _directionResolver.ResolveDirection();
            if (dir == AttackDirection.None)
                dir = _lastDirection;
            else
                _lastDirection = dir;

            float target = dir switch
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

        public void TryAttack()
        {
            if (_currentWeapon == null)
                return;

            if (_isAttacking)
                return;

            AttackDirection dir = _directionResolver.ResolveDirection();
            if (dir == AttackDirection.None)
                dir = _lastDirection;
            else
                _lastDirection = dir;

            _isAttacking = true;

            int index = dir switch
            {
                AttackDirection.Left => 0,
                AttackDirection.Up => 1,
                AttackDirection.Right => 2,
                _ => 1
            };

            _animator.SetInteger("AttackIndex", index);
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
            _currentWeapon.StopHit();
        }

        public void EquipWeapon(WeaponBase weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.Initialize(this, _hitDetector);
        }

        public void OnHitBlocked()
        {
            _isAttacking = false;
            _currentWeapon.StopHit();
            _animator.SetTrigger("Stunned");
        }
    }
}
