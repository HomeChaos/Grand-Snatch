using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.MainCore
{
    class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private float _delayBeforeMoveing = 0.05f;
        [SerializeField] private PositionLimit _positionLimit;

        private Vector2 _delta;
        private bool _isMoving;
        private float _moveStartTime;

        private void LateUpdate()
        {
            if (_isMoving && (Time.time - _moveStartTime > _delayBeforeMoveing))
            {
                var position = transform.right * (_delta.x * -_movementSpeed);
                position += transform.up * (_delta.y * -_movementSpeed);
                transform.position += position * Time.deltaTime;
                ClampCamera();
            }
        }   

        public void OnLook(InputAction.CallbackContext context)
        {
            _delta = context.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _isMoving = context.started || context.performed;
            _moveStartTime = Time.time;
        }
        
        private void ClampCamera()
        {
            transform.position = new Vector3(
                   Mathf.Clamp(transform.position.x, _positionLimit.XLimit.x, _positionLimit.XLimit.y),
                   Mathf.Clamp(transform.position.y, _positionLimit.YLimit.x, _positionLimit.YLimit.y),
                   Mathf.Clamp(transform.position.z, _positionLimit.ZLimit.x, _positionLimit.ZLimit.y));
        }
    }

    [System.Serializable]
    public class PositionLimit
    {
        [SerializeField] private Vector2 _xLimit;
        [SerializeField] private Vector2 _yLimit;
        [SerializeField] private Vector2 _zLimit;

        public Vector2 XLimit => _xLimit;
        public Vector2 YLimit => _yLimit;
        public Vector2 ZLimit => _zLimit;
    }
}