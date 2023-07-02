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
        [SerializeField] private Button _resetData;

        private void OnEnable()
        {
            _musicToggle.StateChange += OnMusicStateChange;
            _SFXToggle.StateChange += OnSFXStateChange;
            _closeButton.onClick.AddListener(CloseWindow);
            _resetData.onClick.AddListener(OnResetData);

            _musicToggle.Init(PlayerData.Instance.IsMusicOn);
            _SFXToggle.Init(PlayerData.Instance.IsSFXOn);
        }

        private void OnDisable()
        {
            _musicToggle.StateChange -= OnMusicStateChange;
            _SFXToggle.StateChange -= OnSFXStateChange;
            _closeButton.onClick.RemoveListener(CloseWindow);
            _resetData.onClick.RemoveListener(OnResetData);

            _musicToggle.Dispose();
            _SFXToggle.Dispose();
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        private void OnResetData()
        {
            PlayerData.Instance.ResetData();
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