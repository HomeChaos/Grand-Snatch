using System;
using Assets.Scripts.Data;

namespace Assets.Scripts.MainCore.MinionScripts
{
    public class MinionSpecifications
    {
        private const int RatioOfSpeedToLevel = 5;
        
        private float _minSpeed;
        private float _maxSpeed;

        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;

        public MinionSpecifications(int startSpeed = 0)
        {
            var minionSpeed = startSpeed + RatioOfSpeedToLevel;
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
