using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MinionSpecifications : MonoBehaviour
    {
        public static MinionSpecifications Instance { get; private set; }

        private const int RatioOfSpeedToLevel = 5;
        
        private float _minSpeed;
        private float _maxSpeed;

        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            var minionSpeed = GameSession.Instance.MinionSpeedLevel + RatioOfSpeedToLevel;
            _minSpeed = minionSpeed - PlayerData.Instance.Config.MinionSpeedRange;
            _maxSpeed = minionSpeed + PlayerData.Instance.Config.MinionSpeedRange;
        }

        public void AddSpeed()
        {
            _minSpeed += PlayerData.Instance.Config.SpeedDelta;
            _maxSpeed += PlayerData.Instance.Config.SpeedDelta;
        }
    }
}
