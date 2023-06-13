using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _SFXToggle;
        [SerializeField] private Button _closeButton;

        public void Init()
        {
            _musicToggle.StateChange += OnMusicStateChange;
            _SFXToggle.StateChange += OnSFXStateChange;
            _closeButton.onClick.AddListener(CloseWindow);
            
            _musicToggle.Init(PlayerData.Instance.IsMusicOn);
            _SFXToggle.Init(PlayerData.Instance.IsSFXOn);
        }

        private void OnDestroy()
        {
            _musicToggle.StateChange -= OnMusicStateChange;
            _SFXToggle.StateChange -= OnSFXStateChange;
            _closeButton.onClick.RemoveListener(CloseWindow);

            _musicToggle.Dispose();
            _SFXToggle.Dispose();
        }
        
        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        
        private void OnMusicStateChange(bool state)
        {
            PlayerData.Instance.IsMusicOn = state;
        }

        private void OnSFXStateChange(bool state)
        {
            PlayerData.Instance.IsSFXOn = state;
        }
    }
}