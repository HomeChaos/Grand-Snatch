using Assets.Scripts.MainCore;
using System;
using Assets.Scripts.MainCore.MinionScripts;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Data
{
    [RequireComponent(typeof(PaymentInformant))]
    public class PaymentSystem : MonoBehaviour
    {
        public static PaymentSystem Instance { get; private set; }

        [SerializeField] private MinionSpawner _minionSpawner;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private ParticleSystem _sellItemParticle;
        [SerializeField] private ProductItem _addMinion;
        [SerializeField] private ProductItem _addSpeed;
        [SerializeField] private ProductItem _income;

        private GameSession _gameSession;
        private PaymentInformant _informant;

        private int _countSoldItems;
        private int _earningsPerLevel = 0;

        public int MaxCountOfItems => _itemManager.MaxCountOfItems;
        public int EarningsPerLevel => _earningsPerLevel;

        public event UnityAction<int, int> ItemSold;
        public event UnityAction AllItemsSold;

        private void OnDestroy()
        {
            _addMinion.Button.onClick.RemoveListener(AddMinion);
            _addSpeed.Button.onClick.RemoveListener(AddSpeed);
            _income.Button.onClick.RemoveListener(AddIncome);
        }

        public void Init(GameSession gameSession)
        {
            if (Instance == null)
                Instance = this;

            _informant = GetComponent<PaymentInformant>();
            _gameSession = gameSession;
            _addMinion.Button.onClick.AddListener(AddMinion);
            _addSpeed.Button.onClick.AddListener(AddSpeed);
            _income.Button.onClick.AddListener(AddIncome);

            InitializeValueForBuy();
        }

        public void SellItem()
        {
            int spread = Random.Range(PlayerData.Instance.Config.MinMoneyRange, PlayerData.Instance.Config.MaxMoneyRange + 2);
            int earnings = _gameSession.CostOfSaleItem + spread;
            _earningsPerLevel += earnings;
            int newValueOfMoney = PlayerData.Instance.Money + earnings;

            if (CheckOutOfRange(newValueOfMoney))
                PlayerData.Instance.Money = int.MaxValue;
            else
                PlayerData.Instance.Money = newValueOfMoney;

            OnSellItem();

            if (_itemManager.MaxCountOfItems == _countSoldItems)
                AllItemsSold?.Invoke();
        }

        private void InitializeValueForBuy()
        {
            int initialNumberOfMinions = 1;
            _addMinion.ImprovementButton.SetValues(initialNumberOfMinions, _gameSession.MinionCost);
            _addSpeed.ImprovementButton.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            _income.ImprovementButton.SetValues(_gameSession.IncomeLevel, _gameSession.IncomeCost);
        }

        private void OnSellItem()
        {
            ItemSold?.Invoke(_itemManager.MaxCountOfItems, ++_countSoldItems);
            Sound.Instance.PlaySFX(CollectionOfSounds.Coin);
            _sellItemParticle.Play();
        }

        private bool CheckOutOfRange(int value) => value < 0;

        private bool TryPay(int payment)
        {
            if (PlayerData.Instance.Money - payment < 0)
                return false;

            PlayerData.Instance.Money -= payment;
            return true;
        }

        private void AddMinion()
        {
            bool isTherePlaceForMinions = _itemManager.CountOfItems > 0 &&
                                          _minionSpawner.CountOfMinions < PlayerData.Instance.Config.MaxCountOfMinions;

            if (isTherePlaceForMinions)
            {
                if (TryPay(_gameSession.MinionCost))
                {
                    _minionSpawner.AddMinion();
                    _gameSession.UpdateMinionCost();
                    _addMinion.ImprovementButton.SetValues(_minionSpawner.CountOfMinions, _gameSession.MinionCost);
                    _addMinion.ImprovementButton.PlayBuyParticle();
                }
                else
                {
                    _informant.ShowInfoNotEnoughMoney(LackOfMoney.Minions);
                }
            }
            else
            {
                _informant.OverflowMinions();
            }
        }

        private void AddSpeed()
        {
            if (TryPay(_gameSession.SpeedCost))
            {
                _gameSession.MinionSpecifications.AddSpeed();
                _gameSession.UpdateSpeedCost();
                _minionSpawner.AddSpeed();
                _addSpeed.ImprovementButton.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
                _addSpeed.ImprovementButton.PlayBuyParticle();
            }
            else
            {
                _informant.ShowInfoNotEnoughMoney(LackOfMoney.Speed);
            }
        }

        private void AddIncome()
        {
            if (TryPay(_gameSession.IncomeCost))
            {
                _gameSession.UpdateItemCost();
                _income.ImprovementButton.SetValues(_gameSession.IncomeLevel, _gameSession.IncomeCost);
                _income.ImprovementButton.PlayBuyParticle();
            }
            else
            {
                _informant.ShowInfoNotEnoughMoney(LackOfMoney.Income);
            }
        }
    }

    [Serializable]
    public class ProductItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private ImprovementButton improvementButton;

        public Button Button => _button;
        public ImprovementButton ImprovementButton => improvementButton;
    }
}