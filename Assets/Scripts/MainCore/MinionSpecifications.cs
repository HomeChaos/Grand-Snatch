using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MinionSpecifications : MonoBehaviour
    {
        public static MinionSpecifications Instance { get; private set; }
        
        [SerializeField] private float _minSpeed = 3;
        [SerializeField] private float _maxSpeed = 6;
        [SerializeField] private float _speedDelta = 0.5f;

        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void AddSpeed()
        {
            _minSpeed += _speedDelta;
            _maxSpeed += _speedDelta;
        }
    }
}
