using Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameUI: MonoBehaviour
    {
        [SerializeField] private PaymentSystem _paymentSystem;
        [SerializeField] private TMP_Text _money;
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _gameOver;

        private void OnEnable()
        {
            _gameOver.SetActive(false);
            _paymentSystem.OnMoneyChanged += UpdateMoney;
            _paymentSystem.ItemSold += OnItemSold;
            _paymentSystem.AllItemsSold += ShowGameOverWindow;
        }

        private void OnDisable()
        {
            _paymentSystem.OnMoneyChanged -= UpdateMoney;
            _paymentSystem.ItemSold -= OnItemSold;
            _paymentSystem.AllItemsSold -= ShowGameOverWindow;
        }

        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        private void UpdateMoney(int money)
        {
            if (money >= int.MaxValue)
                _money.text = "MAX MONEY $";
            else
                _money.text = $"{NumberSeparator.SplitNumber(money)} $";
        }        

        private void OnItemSold(int maxValue, int newValue)
        {
            _slider.value = (float) newValue / maxValue;
        }

        private void ShowGameOverWindow()
        {
            _gameOver.SetActive(true);
        }
    }
}
