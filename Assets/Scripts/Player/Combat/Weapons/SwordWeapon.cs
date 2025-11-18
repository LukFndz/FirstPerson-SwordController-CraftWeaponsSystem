using FP.Player.Combat.Attack;
using FP.Player.Combat.Hit;
using FP.Player.Combat.Weapon;
using UnityEngine;

namespace FP.Player.Combat.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Sword")]
    public sealed class SwordWeapon : WeaponBase
    {
        public override void PerformAttack(AttackDirection direction, HitDetector hitDetector)
        {
            base.PerformAttack(direction, hitDetector);

            if (direction == AttackDirection.None)
                return;

            AttackData attackData = new AttackData(
                direction,
                Damage,
                false,
                Owner
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
