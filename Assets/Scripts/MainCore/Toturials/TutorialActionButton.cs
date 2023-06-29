using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.MainCore.Toturials
{
    public class TutorialActionButton : TutorialAction
    {
        [SerializeField] private Button _targetButton;

        private void OnDestroy()
        {
            _targetButton.onClick.RemoveListener(OnButtonClicked);
        }

        public override void StartTutorial()
        {
            _message.SetActive(true);
            _targetButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _targetButton.onClick.RemoveListener(OnButtonClicked);
            _message.SetActive(false);
            CallTutorialComplete();
        }
    }
}