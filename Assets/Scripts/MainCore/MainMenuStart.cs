using System;
using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using TMPro;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MainMenuStart : MonoBehaviour
    {
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _money;
        
        private void Awake()
        {
            ApplyGameSettings();
        }

        private void Start()
        {
            Sound.Instance.PlayBackgroundMusic(CollectionOfSounds.MainMenu);
        }

        private void OnDestroy()
        {
            PlayerData.Instance.SaveData();
        }

        private void ApplyGameSettings()
        {
            _money.text = NumberSeparator.SplitNumber(PlayerData.Instance.Money);
            _localizer.Init();
            _settingsWindow.Init();
            
        }
    }
}