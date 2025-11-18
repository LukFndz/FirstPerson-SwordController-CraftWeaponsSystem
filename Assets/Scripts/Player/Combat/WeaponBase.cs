using FP.Player.Combat.Attack;
using FP.Player.Combat.Hit;
using UnityEngine;

namespace FP.Player.Combat.Weapon
{
    public abstract class WeaponBase : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private float _damage;

        [Header("Hit Detection")]
        private HitDetector _hitDetector;

        public float Damage => _damage;
        public HitDetector HitDetector => _hitDetector;

        public virtual void PerformAttack(AttackDirection direction, HitDetector hitDetector)
        {
            _hitDetector = hitDetector;
        }
    }
}
