using FP.Player.Combat;

namespace FP.Player.Combat.Attack
{
    /// <summary>
    /// Data used to describe a melee attack. 
    /// Includes direction, damage, blocked state and source controller.
    /// </summary>
    public readonly struct AttackData
    {
        public AttackDirection Direction { get; }
        public float Damage { get; }
        public bool WasBlocked { get; }
        public WeaponController Source { get; }

        public AttackData(
            AttackDirection direction,
            float damage,
            bool wasBlocked = false,
            WeaponController source = null)
        {
            Direction = direction;
            Damage = damage;
            WasBlocked = wasBlocked;
            Source = source;
        }

        public AttackData WithBlocked()
        {
            return new AttackData(Direction, Damage, true, Source);
        }

        public AttackData WithSource(WeaponController source)
        {
            return new AttackData(Direction, Damage, WasBlocked, source);
        }
    }
}
