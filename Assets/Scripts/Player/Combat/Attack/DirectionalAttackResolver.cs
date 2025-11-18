using UnityEngine;

namespace FP.Player.Combat.Attack
{
    /// <summary>
    /// Resolves the player's attack direction based on recent mouse input.
    /// Provides simple directional detection: Up, Down, Left, Right.
    /// </summary>
    public class DirectionalAttackResolver
    {
        private const float _minThreshold = 2f;
        private Vector2 _lastMouseDelta;

        public void UpdateMouseDelta(Vector2 delta)
        {
            _lastMouseDelta = delta;
        }

        public AttackDirection ResolveDirection()
        {
            if (_lastMouseDelta == Vector2.zero)
                return AttackDirection.None;

            float absX = Mathf.Abs(_lastMouseDelta.x);
            float absY = Mathf.Abs(_lastMouseDelta.y);

            if (absX < _minThreshold && absY < _minThreshold)
                return AttackDirection.None;

            if (absX > absY)
                return _lastMouseDelta.x > 0f ? AttackDirection.Right : AttackDirection.Left;

            return _lastMouseDelta.y > 0f ? AttackDirection.Up : AttackDirection.None;
        }
    }
}
