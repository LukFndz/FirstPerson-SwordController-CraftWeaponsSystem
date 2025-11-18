using UnityEngine;

namespace FP.Player.Combat.Attack
{
    /// <summary>
    /// Represents a single attack instance containing all necessary data
    /// such as damage, direction, animation clip and attack range.
    /// </summary>
    public sealed class AttackData
    {
        public AttackDirection Direction { get; }
        public float Damage { get; }

        public AttackData(
            AttackDirection direction,
            float damage)
        {
            Direction = direction;
            Damage = damage;
        }
    }
}
