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
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private ProductItem _addMinion;
        [SerializeField] private ProductItem _addSpeed;
        [SerializeField] private ProductItem _income;
        
        private int _countOfMinion;
        private int _minionSpeed;
        private int _costOfItem;

        private int _maxCountOfItems;
        private int _countSoldItems;

        public event UnityAction<int> OnMoneyChanged;
        public event UnityAction<int, int> ItemSold;
        public event UnityAction AllItemsSold;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void OnEnable()
        {
            _addMinion.Button.onClick.AddListener(AddMinion);
            _addSpeed.Button.onClick.AddListener(AddSpeed);
            _income.Button.onClick.AddListener(UpdateCost);
        }

        private void OnDisable()
        {
            _addMinion.Button.onClick.RemoveListener(AddMinion);
            _addSpeed.Button.onClick.RemoveListener(AddSpeed);
            _income.Button.onClick.RemoveListener(UpdateCost);
        }

        private void Start()
        {
            Invoke(nameof(InitNewValues), 0.4f);
        }

        private void InitNewValues()
        {
            OnMoneyChanged?.Invoke(PlayerData.Instance.Money);
            
            _addMinion.Product.SetValues(1, _gameSession.MinionCost);
            _addSpeed.Product.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            _income.Product.SetValues(_gameSession.IncomeLevel, _gameSession.CostOfUpdateItem);

            _maxCountOfItems = _itemManager.CountOfItems;
            ItemSold?.Invoke(_maxCountOfItems, _countSoldItems);
        }

        public void SellItem()
        {
            int spread = Random.Range(PlayerData.Instance.Config.MinMoneyRange, PlayerData.Instance.Config.MaxMoneyRange + 2);
            int newValueOfMoney = PlayerData.Instance.Money + _gameSession.CostOfSaleItem + spread;
            
            if (newValueOfMoney < 0)
                PlayerData.Instance.Money = int.MaxValue;
            else
                PlayerData.Instance.Money = newValueOfMoney;
            
            OnMoneyChanged?.Invoke(PlayerData.Instance.Money);
            ItemSold?.Invoke(_maxCountOfItems, ++_countSoldItems);
            
            if (_maxCountOfItems == _countSoldItems)
                AllItemsSold?.Invoke();
        }

        private bool TryPay(int payment)
        {
            if (PlayerData.Instance.Money - payment < 0.01f)
                return false;
            
            PlayerData.Instance.Money -= payment;

            OnMoneyChanged?.Invoke(PlayerData.Instance.Money);
            return true;
        }

        private void AddMinion()
        {
            bool conditionForTheUpdate = _itemManager.CountOfItems > 0 &&
                                         _minionSpawner.CountOfMinions < PlayerData.Instance.Config.MaxCountOfMinions &&
                                         TryPay(_gameSession.MinionCost);
            
            if (conditionForTheUpdate)
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
                _minionSpawner.AddSpeed();
                _gameSession.UpdateSpeedCost();
                _addSpeed.Product.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            }
        }

        private void UpdateCost()
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
