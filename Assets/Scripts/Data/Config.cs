using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config", order = 52)]
    public class Config : ScriptableObject
    {
        [Header("Money")] 
        [SerializeField] private int _minMoneyRange = -2;
        [SerializeField] private int _maxMoneyRange = 2;
        [SerializeField] private int _ratioOfMoneyToLevel = 10;
        
        [Header("Starting price of update")] 
        [SerializeField] private int _startMinionCost = 10;
        [SerializeField] private int _startSpeedCost = 11;
        [SerializeField] private int _startCostOfUpdateItem = 11;
        
        [Header("Calculate new cost for update")] 
        [SerializeField] private float _factorMinionCost = 1.5f;
        [SerializeField] private float _factorSpeedCost = 2.5f;
        [SerializeField] private float _factorItemCost = 3.7f;

        [Header("Minion Specifications")] 
        [SerializeField] private int _minionSpeedRange = 2;
        [SerializeField] private float _speedDelta = 0.5f;
        [SerializeField] private int _maxCountOfMinions = 20;

        [Header("Human Specifications")] 
        [SerializeField] private int _humanCountToSpawn = 3;
        [SerializeField] private int _humanSpeed = 3;

        public int MaxCountOfMinions => _maxCountOfMinions;

        public float SpeedDelta => _speedDelta;

        public int MinionSpeedRange => _minionSpeedRange;

        public float FactorMinionCost => _factorMinionCost;

        public float FactorSpeedCost => _factorSpeedCost;

        public float FactorItemCost => _factorItemCost;

        public int RatioOfMoneyToLevel => _ratioOfMoneyToLevel;

        public int StartMinionCost => _startMinionCost;

        public int StartSpeedCost => _startSpeedCost;

        public int StartCostOfUpdateItem => _startCostOfUpdateItem;

        public int MinMoneyRange => _minMoneyRange;

        public int MaxMoneyRange => _maxMoneyRange;

        public int HumanCountToSpawn => _humanCountToSpawn;

        public int HumanSpeed => _humanSpeed;
    }
}