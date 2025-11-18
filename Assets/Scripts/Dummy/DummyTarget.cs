using UnityEngine;
using FP.Player.Combat.Hit;
using FP.Player.Combat.Attack;

namespace FP.Dummy
{
    /// <summary>
    /// Simple damage receiver used for melee weapon testing.
    /// Can block attacks based on shield direction.
    /// </summary>
    public sealed class DummyTarget : MonoBehaviour, IHitReceiver
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private ShieldDirection _shieldDirection = ShieldDirection.None;

        public void ReceiveHit(AttackData attackData)
        {
            if (IsBlocked(attackData.Direction))
                return;

            _health -= attackData.Damage;
            if (_health <= 0f)
                HandleDeath();
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

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
    }
}
