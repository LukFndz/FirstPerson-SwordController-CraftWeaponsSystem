using UnityEngine;
using FP.Player.Combat.Attack;

namespace FP.Player.Combat.Hit
{
    /// <summary>
    /// Detects hits within a defined area and notifies hit receivers.
    /// </summary>
    public sealed class HitDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private Transform _origin;
        [SerializeField] private Vector3 _size;

        private readonly Collider[] _results = new Collider[8];
        private bool _isActive;
        private AttackData _data;

        public void StartHit(AttackData data)
        {
            _isActive = true;
            _data = data;
        }

        public void StopHit()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (_isActive)
                PerformHitInternal();
        }

        private void PerformHitInternal()
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
                if (_results[i].TryGetComponent<IHitReceiver>(out var receiver))
                    receiver.ReceiveHit(_data);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_origin == null)
                return;

            Gizmos.color = Color.red;
            Matrix4x4 old = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(
                _origin.position,
                _origin.rotation,
                _size
            );

            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = old;
        }
    }
}
