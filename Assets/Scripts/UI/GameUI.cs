using Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;
        [SerializeField] private Slider _levelInfoSlider;
        [FormerlySerializedAs("_levelInfoText")] [SerializeField] private TMP_Text _itemsInfoText;
        [SerializeField] private TMP_Text _levelNumber;
        [SerializeField] private GameObject _gameOver;
        
        private PaymentSystem _paymentSystem;

        private void OnDisable()
        {
            PlayerData.Instance.MoneyChanged -= UpdateMoney;
            
            if (_paymentSystem != null)
            {
                _paymentSystem.ItemSold -= OnItemSold;
                _paymentSystem.AllItemsSold -= ShowGameOverWindow;
            }
        }

        public void Init(PaymentSystem paymentSystem)
        {
            _gameOver.SetActive(false);
            
            _paymentSystem = paymentSystem;
            _paymentSystem.ItemSold += OnItemSold;
            _paymentSystem.AllItemsSold += ShowGameOverWindow;
            OnItemSold(_paymentSystem.MaxCountOfItems, 0);

            PlayerData.Instance.MoneyChanged += UpdateMoney;
            UpdateMoney(PlayerData.Instance.Money);
            
            _levelNumber.text = PlayerData.Instance.Level.ToString();
        }

        private void UpdateMoney(int money)
        {
            if (money >= int.MaxValue)
                _money.text = "MAX $";
            else
                _money.text = $"{NumberSeparator.SplitNumber(money)} $";
        }        

        private void OnItemSold(int maxValue, int newValue)
        {
            _levelInfoSlider.value = (float) newValue / maxValue;
            _itemsInfoText.text = $"{newValue}/{maxValue}";
        }

        private void ShowGameOverWindow()
        {
            _gameOver.SetActive(true);
        }
    }
}
