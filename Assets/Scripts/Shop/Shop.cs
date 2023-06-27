using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.YandexSDK;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private PriceList _priceList;
        [SerializeField] private GameObject _productTemplate;
        [SerializeField] private GameObject _conteiner;
        [SerializeField] private MessageBox _messageBoxWatchAd;
        [SerializeField] private MessageBox _messageBoxBuy;
        [SerializeField] private MessageBox _messageBoxNotEnoughMoney;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        [Space] [Header("AD")]
        [SerializeField] private YandexAd _yandexAd;

        private List<Product> _products = new List<Product>();
        private Product _currentProduct;
        private Price _currentPrice;

        private void OnDestroy()
        {
            foreach (var product in _products)
            {
                product.Clicked -= OnProductClicked;
            }

            _messageBoxBuy.IsConfirmAction -= ProcessingReceivedFromMessageBox;
            _startButton.onClick.RemoveListener(StartGame);
            _exitButton.onClick.RemoveListener(Exit);
        }

        public void Init()
        {
            for (int i = 0; i < _priceList.Prices.Count; i++)
            {
                var item = _priceList.Prices[i];
                
                var product = Instantiate(_productTemplate, _conteiner.transform).GetComponent<Product>();
                product.Init(item);
                product.Clicked += OnProductClicked;
                
                _products.Add(product);
            }
            
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(Exit);
        }

        private void StartGame()
        {
            Level_1.Load();
        }

        private void Exit()
        {
            MainMenu.Load();
        }

        private void OnProductClicked(Product productItem, Price price)
        {
            CarType carType = price.CarType;
            _currentProduct = productItem;
            _currentPrice = price;

            if (PlayerData.Instance.ConditionsForCars[carType] == 0)
                ChangeSelectCar();
            else if (price.IsBuyForAd) 
                ShowMessageBoxWatchAd();
            else
                TryBuyCar();
        }

        private void ChangeSelectCar()
        {
            foreach (var product in _products)
                product.UnSelect();
                
            _currentProduct.Select();
            PlayerData.Instance.SelectedCar = (int)_currentPrice.CarType;
        }

        private void TryBuyCar()
        {
            bool сanPay = PlayerData.Instance.Money - _currentPrice.Cost >= 0;
            
            if (сanPay)
                ShowMessageBoxBuy();
            else
                _messageBoxNotEnoughMoney.ShowMessageBox();
        }

        private void ShowMessageBoxWatchAd()
        {
            _messageBoxWatchAd.ShowMessageBox();
            _messageBoxWatchAd.IsConfirmAction += ProcessingReceivedFromMessageBox;
        }

        private void ShowMessageBoxBuy()
        {
            _messageBoxBuy.ShowMessageBox();
            _messageBoxBuy.IsConfirmAction += ProcessingReceivedFromMessageBox;
        }

        private void ProcessingReceivedFromMessageBox(bool response)
        {
            _messageBoxWatchAd.IsConfirmAction -= ProcessingReceivedFromMessageBox;
            _messageBoxBuy.IsConfirmAction -= ProcessingReceivedFromMessageBox;
            
            if (response)
            {

                if (_currentPrice.IsBuyForAd)
                {
                    _yandexAd.OnShowVideoButtonClick(OnRewarded);
                }
                else
                {
                    PlayerData.Instance.ChangeConditionForCar(_currentPrice.CarType);
                    PlayerData.Instance.Money -= _currentPrice.Cost;
                    _currentProduct.Unlock();
                }
            }
        }

        private void OnRewarded()
        {
            Debug.Log("[!] Я получил награду!");
            PlayerData.Instance.ChangeConditionForCar(_currentPrice.CarType);
            _currentProduct.UpdateCostText();
                    
            if (PlayerData.Instance.ConditionsForCars[_currentPrice.CarType] == 0)
            {
                _currentProduct.Unlock();
            }
        }
    }
}