using Assets.Scripts.MainCore.MinionScripts;
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
        private int _incomeCost;
        private int _incomeLevel = 1;

        private MinionSpecifications _minionSpecifications;

        public int MinionCost => _minionCost;
        public int SpeedCost => _speedCost;
        public int MinionSpeedLevel => _minionSpeedLevel;
        public int CostOfSaleItem => _incomeLevel * PlayerData.Instance.Config.RatioOfMoneyToLevel;
        public int IncomeCost => _incomeCost;
        public int IncomeLevel => _incomeLevel;
        public MinionSpecifications MinionSpecifications => _minionSpecifications;

        public void Init()
        {
            if (Instance == null)
                Instance = this;

            _minionCost = PlayerData.Instance.Config.StartMinionCost;
            _speedCost = PlayerData.Instance.Config.StartSpeedCost;
            _incomeCost = PlayerData.Instance.Config.StartCostIncome;
            _minionSpecifications = new MinionSpecifications();
        }

        public void UpdateMinionCost()
        {
            var level = PlayerData.Instance.Level;
            var config = PlayerData.Instance.Config;
            _minionCost += (int) (Mathf.Pow(2, _minionCostLevel) * (int) (level * config.FactorMinionCost));
            _minionCostLevel++;
        }

        public void UpdateSpeedCost()
        {
            var level = PlayerData.Instance.Level;
            var config = PlayerData.Instance.Config;
            _speedCost += (int) (Mathf.Pow(2, _minionSpeedLevel) * (int) (level * config.FactorSpeedCost));
            _minionSpeedLevel++;
        }

        public void UpdateItemCost()
        {
            var level = PlayerData.Instance.Level;
            var config = PlayerData.Instance.Config;
            _incomeCost += (int) (Mathf.Pow(2, _incomeLevel) * (int) (level * config.FactorIncome));
            _incomeLevel++;
        }
    }
}