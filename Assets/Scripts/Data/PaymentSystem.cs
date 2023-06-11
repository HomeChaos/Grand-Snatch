using Assets.Scripts.MainCore;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Data
{
    public class PaymentSystem : MonoBehaviour
    {
        public static PaymentSystem Instance { get; private set; }
        
        [SerializeField] private MinionSpawner _minionSpawner;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private ProductItem _addMinion;
        [SerializeField] private ProductItem _addSpeed;
        [SerializeField] private ProductItem _income;

        private GameSession _gameSession;
        
        private int _countSoldItems;
        
        public int MaxCountOfItems => _itemManager.MaxCountOfItems;
        
        public event UnityAction<int, int> ItemSold;
        public event UnityAction AllItemsSold;

        public void Init(GameSession gameSession)
        {
            if (Instance == null)
                Instance = this;

            _gameSession = gameSession;
            _addMinion.Button.onClick.AddListener(AddMinion);
            _addSpeed.Button.onClick.AddListener(AddSpeed);
            _income.Button.onClick.AddListener(AddIncome);

            InitValueForBuy();
        }

        private void OnDisable()
        {
            _addMinion.Button.onClick.RemoveListener(AddMinion);
            _addSpeed.Button.onClick.RemoveListener(AddSpeed);
            _income.Button.onClick.RemoveListener(AddIncome);
        }

        public void SellItem()
        {
            int spread = Random.Range(PlayerData.Instance.Config.MinMoneyRange, PlayerData.Instance.Config.MaxMoneyRange + 2);
            int newValueOfMoney = PlayerData.Instance.Money + _gameSession.CostOfSaleItem + spread;
            
            if (newValueOfMoney < 0)
                PlayerData.Instance.Money = int.MaxValue;
            else
                PlayerData.Instance.Money = newValueOfMoney;

            ItemSold?.Invoke(_itemManager.MaxCountOfItems, ++_countSoldItems);
            
            if (_itemManager.MaxCountOfItems == _countSoldItems)
                AllItemsSold?.Invoke();
        }

        private void InitValueForBuy()
        {
            _addMinion.Product.SetValues(1, _gameSession.MinionCost);
            _addSpeed.Product.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            _income.Product.SetValues(_gameSession.IncomeLevel, _gameSession.CostOfUpdateItem);
        }

        private bool TryPay(int payment)
        {
            if (PlayerData.Instance.Money - payment < 0.01f)
                return false;
            
            PlayerData.Instance.Money -= payment;
            return true;
        }

        private void AddMinion()
        {
            bool conditionForUpdate = _itemManager.CountOfItems > 0 &&
                                         _minionSpawner.CountOfMinions < PlayerData.Instance.Config.MaxCountOfMinions &&
                                         TryPay(_gameSession.MinionCost);
            
            if (conditionForUpdate)
            {
                _minionSpawner.AddMinion();
                _gameSession.UpdateMinionCost();
                _addMinion.Product.SetValues(_minionSpawner.CountOfMinions, _gameSession.MinionCost);
            }
        }

        private void AddSpeed()
        {
            if (TryPay(_gameSession.SpeedCost))
            {
                _gameSession.MinionSpecifications.AddSpeed();
                _gameSession.UpdateSpeedCost();
                _minionSpawner.AddSpeed();
                _addSpeed.Product.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            }
        }

        private void AddIncome()
        {
            if (TryPay(_gameSession.CostOfUpdateItem))
            {
                _gameSession.UpdateItemCost();
                _income.Product.SetValues(_gameSession.IncomeLevel, _gameSession.CostOfUpdateItem);
            }
        }
    }

    [Serializable]
    public class ProductItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private Product _product;

        public Button Button => _button;
        public Product Product => _product;
    }
}
