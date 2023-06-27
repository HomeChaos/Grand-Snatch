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
        [SerializeField] private Sound _sound;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private Shop _shop;
        [SerializeField] private TMP_Text _money;

        private void Awake()
        {
            ApplyGameSettings();
        }
        
        private void OnDestroy()
        {
            PlayerData.Instance.SaveData();
            PlayerData.Instance.MoneyChanged -= InstanceOnMoneyChanged;
        }

        private void ApplyGameSettings()
        {
            PlayerData.Instance.MoneyChanged += InstanceOnMoneyChanged;
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