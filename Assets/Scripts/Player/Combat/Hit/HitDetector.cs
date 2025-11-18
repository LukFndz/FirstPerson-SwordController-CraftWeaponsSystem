using UnityEngine;
using FP.Player.Combat.Attack;

namespace FP.Player.Combat.Hit
{
    /// <summary>
    /// Detects hits for melee weapons using a box-shaped overlap volume.
    /// Applies damage to any object implementing IHitReceiver.
    /// </summary>
    public sealed class HitDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private Transform _origin;
        [SerializeField] private Vector3 _size;

        private readonly Collider[] _results = new Collider[8];

        /// <summary>
        /// Performs a hit detection and applies damage to valid receivers.
        /// </summary>
        public void PerformHit(AttackData attackData)
        {
            if (_origin == null)
                return;

            int count = Physics.OverlapBoxNonAlloc(
                _origin.position,
                _size * 0.5f,
                _results,
                _origin.rotation,
                _hitMask
            );

            for (int i = 0; i < count; i++)
            {
                Collider col = _results[i];
                if (!col.TryGetComponent<IHitReceiver>(out var receiver))
                    continue;

                receiver.ReceiveHit(attackData);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_origin == null)
                return;

            var oldMatrix = Gizmos.matrix;

            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(
                _origin.position,
                _origin.rotation,
                _size
            );

            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldMatrix;
        }
    }
}
