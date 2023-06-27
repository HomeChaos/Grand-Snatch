using System.Collections;
using Agava.YandexGames;
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
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private Sound _sound;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _money;
        
        private void Awake()
        {
            StartCoroutine(WaitForYandexInitialize());
        }
        
        private void OnDestroy()
        {
            PlayerData.Instance.SaveData();
        }

        private IEnumerator WaitForYandexInitialize()
        {
            while (YandexGamesSdk.IsInitialized == false)
            {
                yield return null;
            }

            StartCoroutine(WaitForLoadPlayerData());
        }

        private IEnumerator WaitForLoadPlayerData()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            bool isPlayerDataLoaded = false;
            
            while (isPlayerDataLoaded == false)
            {
                if (PlayerData.Instance != null)
                {
                    isPlayerDataLoaded = PlayerData.Instance.IsDataLoaded;
                }
                
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