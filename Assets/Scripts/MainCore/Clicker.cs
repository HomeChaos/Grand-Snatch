using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.MainCore
{
    public class Clicker: MonoBehaviour
    {
        [SerializeField] private float _delayAfterClick = 0.5f;

        private float _lastTimeClick = 0f;
        private bool _pointerOverUI;
        private bool _isFirstClick = true;

        public event UnityAction OnClick;       

        public void Click(InputAction.CallbackContext context)
        {
            if (_pointerOverUI)
            {
                return;
            }
            
            if (Time.time - _lastTimeClick > _delayAfterClick)
            {
                _isFirstClick = true;
            }
            
            if (context.started)
            {
                _lastTimeClick = Time.time;
            }
            else if (context.canceled)
            {
                if (_isFirstClick)
                {
                    _isFirstClick = false;
                }
                else if(_pointerOverUI == false && Time.time - _lastTimeClick < _delayAfterClick)
                {
                    OnClick?.Invoke();
                }
            }
        }

        private void FixedUpdate()
        {
            _pointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }
}
