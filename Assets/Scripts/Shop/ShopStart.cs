using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using TMPro;
using UI.Localization;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class ShopStart : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Sound _sound;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private Shop _shop;
        [SerializeField] private TMP_Text _money;

        private void Awake()
        {
            _playerData.Init();
            StartCoroutine(WaitForLoadPlayerData());
        }
        
        private void OnDestroy()
        {
            _playerData.SaveData();
            _playerData.MoneyChanged -= InstanceOnMoneyChanged;
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
            _playerData.MoneyChanged += InstanceOnMoneyChanged;
            _localizer.Init();
            _shop.Init();
            _money.text = NumberSeparator.SplitNumber(PlayerData.Instance.Money);
            _sound.Init();
            _sound.PlayBackgroundMusic(CollectionOfSounds.MainMenu);
        }

        private void InstanceOnMoneyChanged(int newValue)
        {
            _money.text = NumberSeparator.SplitNumber(PlayerData.Instance.Money);
        }
    }
}