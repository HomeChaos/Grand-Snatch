using UnityEngine;

namespace Assets.Scripts.MainCore.Toturials
{
    public class TutorialActionCliker : TutorialAction
    {
        [SerializeField] private Clicker _clicker;

        private void OnDestroy()
        {
            _clicker.OnClick -= OnUserClicked;
        }

        public override void StartTutorial()
        {
            _message.SetActive(true);
            _clicker.OnClick += OnUserClicked;
        }

        private void OnUserClicked()
        {
            _clicker.OnClick -= OnUserClicked;
            _message.SetActive(false);
            CallTutorialComplete();
        }
    }
}