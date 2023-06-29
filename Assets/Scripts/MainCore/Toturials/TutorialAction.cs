using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore.Toturials
{
    public abstract class TutorialAction : MonoBehaviour, ITutorialAction
    {
        [SerializeField] protected GameObject _message;
        
        public event UnityAction OnTutorialComplete;

        private void Awake()
        {
            _message.SetActive(false);
        }

        public abstract void StartTutorial();

        protected void CallTutorialComplete()
        {
            OnTutorialComplete?.Invoke();
        }
    }
}