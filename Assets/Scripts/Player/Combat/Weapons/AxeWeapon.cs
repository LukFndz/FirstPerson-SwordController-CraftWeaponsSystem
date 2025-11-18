using UnityEngine;
using FP.Player.Combat.Attack;
using FP.Player.Combat.Weapon;
using FP.Player.Combat.Hit;

namespace FP.Player.Combat.Weapons
{
    /// <summary>
    /// Concrete axe weapon implementation.
    /// Deals slower but stronger directional melee attacks.
    /// </summary>
    [CreateAssetMenu(menuName = "Weapons/Axe")]
    public sealed class AxeWeapon : WeaponBase
    {
        public override void PerformAttack(AttackDirection direction, HitDetector hitDetector)
        {
            base.PerformAttack(direction, hitDetector);

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

            HitDetector.StartHit(attackData);
        }
    }
}
