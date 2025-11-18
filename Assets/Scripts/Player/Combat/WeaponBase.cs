using FP.Player.Combat.Attack;
using FP.Player.Combat.Hit;
using UnityEngine;

namespace FP.Player.Combat.Weapon
{
    /// <summary>
    /// Base class for all weapons in the game.
    /// </summary>
    public abstract class WeaponBase : ScriptableObject
    {
        public float Damage => _damage;
        public WeaponController Owner { get; private set; }
        public HitDetector HitDetector { get; private set; }

        [SerializeField] private float _damage;

        /// <summary>
        /// Injects dependencies when a weapon is equipped at runtime.
        /// </summary>
        public void Initialize(WeaponController owner, HitDetector hitDetector)
        {
            Owner = owner;
            HitDetector = hitDetector;
        }

        public virtual void PerformAttack(AttackDirection direction, HitDetector detector)
        {
            HitDetector = detector;
        }

        public virtual void StopHit()
        {
            HitDetector?.StopHit();
        }
    }
}
