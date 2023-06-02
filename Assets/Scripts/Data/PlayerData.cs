using UnityEngine;

namespace Assets.Scripts.Data
{
    public class PlayerData: MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }

        [SerializeField] private int _money;
        [SerializeField] private int _level;
        [Space]
        [SerializeField] private bool _isMusicOn;
        [SerializeField] private bool _isSFXOn;
        [SerializeField] private Config _config;

        public Config Config => _config;

        public int Money => _money;
        public int Level => _level;
        public bool IsMusicOn => _isMusicOn;
        public bool SFXOn => _isSFXOn;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            LoadData();
        }

        private void LoadData()
        {
            _money = int.MaxValue;
            _level = 1;

            _isMusicOn = true;
            _isSFXOn = true;
        }
    }
}
