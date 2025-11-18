using UnityEngine;
using FP.Player.Combat.Attack;
using FP.Player.Combat.Weapon;
using FP.Player.Combat.Hit;

namespace FP.Player.Combat.Weapons
{
    /// <summary>
    /// Concrete sword weapon implementation.
    /// Generates attack data based on the direction and
    /// performs a melee hit scan using the HitDetector.
    /// </summary>
    [CreateAssetMenu(menuName = "Weapons/Sword")]
    public sealed class SwordWeapon : WeaponBase
    {
        public override void PerformAttack(AttackDirection direction, HitDetector hitDetector)
        {
            if (direction == AttackDirection.None)
                return;

            AttackData attackData = new AttackData(
                direction,
                Damage
            );

            ExecuteAttack(attackData);
        }

        private void ExecuteAttack(AttackData attackData)
        {
            if (HitDetector == null)
                return;

            HitDetector.PerformHit(attackData);
        }
    }
}
