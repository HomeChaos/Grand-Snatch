using Assets.Scripts.Data;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _SFXToggle;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _closeButton;

        private void OnEnable()
        {            
            _musicToggle.StateChange += OnMusicStateChange;
            _SFXToggle.StateChange += OnSFXStateChange;
            
            _shopButton.onClick.AddListener(LoadShop);
            _exitButton.onClick.AddListener(ExitGame);
            _closeButton.onClick.AddListener(CloseWindow);
            
            _musicToggle.Init(PlayerData.Instance.IsMusicOn);
            _SFXToggle.Init(PlayerData.Instance.IsSFXOn);
            
            Time.timeScale = 0;
        }

        private void OnDisable()
        {            
            _musicToggle.StateChange -= OnMusicStateChange;
            _SFXToggle.StateChange -= OnSFXStateChange;
            
            _musicToggle.Dispose();
            _SFXToggle.Dispose();
            
            _shopButton.onClick.RemoveListener(LoadShop);
            _exitButton.onClick.RemoveListener(ExitGame);
            _closeButton.onClick.RemoveListener(CloseWindow);
            
            Time.timeScale = 1f;
        }

        private void LoadShop()
        {
            ShopLevel.Load();
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        private void ExitGame()
        {
            IJunior.TypedScenes.MainMenu.Load();
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