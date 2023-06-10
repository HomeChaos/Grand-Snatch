using UnityEngine;

namespace Assets.Scripts.Data
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }

        private int _minionCost;
        private int _minionCostLevel;

        private int _speedCost;
        private int _minionSpeedLevel;

        private int _costOfUpdateItem;
        private int _incomeLevel = 1;

        public int MinionCost => _minionCost;
        public int SpeedCost => _speedCost;
        public int MinionSpeedLevel => _minionSpeedLevel;
        public int CostOfSaleItem => _incomeLevel * PlayerData.Instance.Config.RatioOfMoneyToLevel;
        public int CostOfUpdateItem => _costOfUpdateItem;
        public int IncomeLevel => _incomeLevel;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _minionCost = PlayerData.Instance.Config.StartMinionCost;
            _speedCost = PlayerData.Instance.Config.StartSpeedCost;
            _costOfUpdateItem = PlayerData.Instance.Config.StartCostOfUpdateItem;
        }

        public void UpdateMinionCost()
        {
            _minionCost += (int) (Mathf.Pow(2, _minionCostLevel) * (int) (PlayerData.Instance.Level * PlayerData.Instance.Config.FactorMinionCost));
            _minionCostLevel++;
        }

        public void UpdateSpeedCost()
        {
            _speedCost += (int) (Mathf.Pow(2, _minionSpeedLevel) * (int) (PlayerData.Instance.Level * PlayerData.Instance.Config.FactorSpeedCost));
            _minionSpeedLevel++;
        }

        public void UpdateItemCost()
        {
            _costOfUpdateItem += (int) (Mathf.Pow(2, _incomeLevel) * (int) (PlayerData.Instance.Level * PlayerData.Instance.Config.FactorItemCost));
            _incomeLevel++;
        }
    }
}