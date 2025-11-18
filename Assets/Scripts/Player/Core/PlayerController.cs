using FP.Player.Combat;
using FP.Player.Utilities;
using UnityEngine;

namespace FP.Player.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerCameraController _cameraController;
        [SerializeField] private HeadbobController _headbob;
        [SerializeField] private WeaponController _weaponController;

        private void Update()
        {
            _movement.Tick();
            _cameraController.Tick();
            _weaponController.Tick();

            float speed = _movement.CurrentSpeed;
            _headbob.Tick(speed);
        }
    }
}
