using Assets.Scripts.MainCore;
using System;
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
        
        public int MaxCountOfItems => _itemManager.MaxCountOfItems;
        
        public event UnityAction<int, int> ItemSold;
        public event UnityAction AllItemsSold;

        public void Init(GameSession gameSession)
        {
            if (Instance == null)
                Instance = this;

            _informant = GetComponent<PaymentInformant>();
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
            Sound.Instance.PlaySFX(CollectionOfSounds.Coin);
            _sellItemParticle.Play();
            
            if (_itemManager.MaxCountOfItems == _countSoldItems)
                AllItemsSold?.Invoke();
        }

        private void InitValueForBuy()
        {
            _addMinion.BoostKey.SetValues(1, _gameSession.MinionCost);
            _addSpeed.BoostKey.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
            _income.BoostKey.SetValues(_gameSession.IncomeLevel, _gameSession.CostOfUpdateItem);
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
            bool isTherePlaceForMinions = _itemManager.CountOfItems > 0 &&
                                    _minionSpawner.CountOfMinions < PlayerData.Instance.Config.MaxCountOfMinions;

            if (isTherePlaceForMinions)
            {
                if (TryPay(_gameSession.MinionCost))
                {
                    _minionSpawner.AddMinion();
                    _gameSession.UpdateMinionCost();
                    _addMinion.BoostKey.SetValues(_minionSpawner.CountOfMinions, _gameSession.MinionCost);
                    _addMinion.BoostKey.Particle.Play();
                }
                else
                {
                    _informant.NotEnoughMoney(LackOfMoney.Minions);
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
                _addSpeed.BoostKey.SetValues(_gameSession.MinionSpeedLevel, _gameSession.SpeedCost);
                _addSpeed.BoostKey.Particle.Play();
            }
            else
            {
                _informant.NotEnoughMoney(LackOfMoney.Speed);
            }
        }

        private void AddIncome()
        {
            if (TryPay(_gameSession.CostOfUpdateItem))
            {
                _gameSession.UpdateItemCost();
                _income.BoostKey.SetValues(_gameSession.IncomeLevel, _gameSession.CostOfUpdateItem);
                _income.BoostKey.Particle.Play();
            }
            else
            {
                _informant.NotEnoughMoney(LackOfMoney.Income);
            }
        }
    }

    [Serializable]
    public class ProductItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private BoostKey boostKey;

        public Button Button => _button;
        public BoostKey BoostKey => boostKey;
    }
}
