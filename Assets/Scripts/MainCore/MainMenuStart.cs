using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using TMPro;
using UI.Localization;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MainMenuStart : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private Sound _sound;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _money;
        
        private void Awake()
        {
            _playerData.Init();
            StartCoroutine(WaitForLoadPlayerData());
        }
        
        private void OnDestroy()
        {
            _playerData.SaveData();
        }

        private IEnumerator WaitForLoadPlayerData()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            
            while (_playerData.IsDataLoaded == false)
            {
                yield return waitForEndOfFrame;
            }
            
            ApplyGameSettings();
        }
        
        private void ApplyGameSettings()
        {
            _money.text = NumberSeparator.SplitNumber(PlayerData.Instance.Money);
            _sound.Init();
            _sound.PlayBackgroundMusic(CollectionOfSounds.MainMenu);
            _localizer.Init();
            _settingsWindow.Init();
        }
    }
}