using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using TMPro;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class ShopStart : MonoBehaviour
    {
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
            InstanceOnMoneyChanged(PlayerData.Instance.Money);
            _localizer.Init();
            _shop.Init();
            Sound.Instance.PlayBackgroundMusic(CollectionOfSounds.MainMenu);
        }

        private void InstanceOnMoneyChanged(int newValue)
        {
            _money.text = NumberSeparator.SplitNumber(PlayerData.Instance.Money);
        }
    }
}