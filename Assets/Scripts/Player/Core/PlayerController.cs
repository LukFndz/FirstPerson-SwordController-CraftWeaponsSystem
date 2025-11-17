using FP.Player.Utilities;
using UnityEngine;

namespace FP.Player.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerCameraController _cameraController;
        [SerializeField] private HeadbobController _headbob;

        private void Update()
        {
            _movement.Tick();
            _cameraController.Tick();

            var speed = _movement.CurrentSpeed;
            _headbob.Tick(speed);
        }
    }
}
