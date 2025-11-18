using UnityEngine;
using FP.Player.Combat.Hit;
using FP.Player.Combat.Attack;

namespace FP.Dummy
{
    /// <summary>
    /// Dummy Targets that look the player and can receive hits.
    /// </summary>
    public sealed class DummyTarget : MonoBehaviour, IHitReceiver
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private ShieldDirection _shieldDirection = ShieldDirection.None;
        [SerializeField] private Transform _target;

        private void Update()
        {
            Vector3 direction = _target.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
            }
        }

        public void ReceiveHit(AttackData attackData)
        {
            if (IsBlocked(attackData.Direction))
            {
                attackData.Source?.OnHitBlocked();
                return;
            }

            _health -= attackData.Damage;
            if (_health <= 0f)
                gameObject.SetActive(false);
        }

        private bool IsBlocked(AttackDirection incoming)
        {
            if (_shieldDirection == ShieldDirection.None)
                return false;

            return incoming switch
            {
                AttackDirection.Up => _shieldDirection == ShieldDirection.Up,
                AttackDirection.Left => _shieldDirection == ShieldDirection.Left,
                AttackDirection.Right => _shieldDirection == ShieldDirection.Right,
                _ => false
            };
        }
    }
}
