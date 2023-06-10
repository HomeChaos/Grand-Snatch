using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [System.Serializable]
    public class Toggle : System.IDisposable
    {
        [SerializeField] private Button _toggleOn;
        [SerializeField] private Button _toggleOff;
        
        public bool State { get; private set; }
        public event UnityAction<bool> StateChange;

        public void Init(bool startState)
        {
            State = startState;
            ChangeState();
            
            _toggleOn.onClick.AddListener(OnClick);
            _toggleOff.onClick.AddListener(OnClick);
        }

        public void Dispose()
        {
            _toggleOn.onClick.RemoveListener(OnClick);
            _toggleOff.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            State = !State;
            ChangeState();
            StateChange?.Invoke(State);
        }

        private void ChangeState()
        {
            _toggleOn.gameObject.SetActive(State);
            _toggleOff.gameObject.SetActive(!State);
        }
    }
}