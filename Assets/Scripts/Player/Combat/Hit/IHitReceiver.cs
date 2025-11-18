using FP.Player.Combat.Attack;

namespace FP.Player.Combat.Hit
{
    /// <summary>
    /// Interface for any object that can receive melee attack hits.
    /// </summary>
    public interface IHitReceiver
    {
        void ReceiveHit(AttackData attackData);
    }
}
