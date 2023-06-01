using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.MainCore
{
    public class Booster: MonoBehaviour
    {
        [SerializeField] private float _delayAfterClick = 0.5f;

        private float _lastClick = 0f;
        private bool _pointerOverUI;
        private bool _isFirstClick = true;

        public event UnityAction OnClick;       

        public void Click(InputAction.CallbackContext context)
        {
            if (Time.time - _lastClick > _delayAfterClick)
            {
                _isFirstClick = true;
            }
            
            if (context.started)
            {
                _lastClick = Time.time;
            }
            else if (context.canceled)
            {
                if (_isFirstClick)
                {
                    _isFirstClick = false;
                }
                else if(_pointerOverUI == false && Time.time - _lastClick < _delayAfterClick)
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
