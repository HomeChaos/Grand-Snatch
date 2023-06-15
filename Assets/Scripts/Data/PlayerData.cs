using System;
using UI.Localization;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerData : IDisposable
    {
        private const string MoneyKey = "Money";
        private const string LevelKey = "Level";
        private const string MusicKey = "Music";
        private const string SFXKey = "SFX";
        private const string LocalizationKey = "Localization";
        
        private const int MoneyDefault = 100;
        private const int LevelDefault = 7;
        private const bool MusicDefault = true;
        private const bool SFXDefault = true;
        
        public static PlayerData Instance { get; private set; }

        [SerializeField] private int _money;
        [SerializeField] private int _level;
        [Space]
        [SerializeField] private bool _isMusicOn;
        [SerializeField] private bool _isSFXOn;
        [SerializeField] private string _currentLocalization;
        [SerializeField] private Config _config;

        public Config Config => _config;

        public int Money
        {
            get
            {
                return _money;
            }
            set
            {
                if (value < 0 && value > Int32.MaxValue)
                    throw new RankException("Incorrect value of money");

                _money = value;
                MoneyChanged?.Invoke(_money);
            }
        }

        public int Level => _level;

        public bool IsMusicOn
        {
            get
            {
                return _isMusicOn;
            }
            set
            {
                _isMusicOn = value;
                MusicStatusChange?.Invoke();
                SaveData();
            }
        }
        
        public bool IsSFXOn {
            get
            {
                return _isSFXOn;
            }
            set
            {
                _isSFXOn = value;
                SFXStatusChange?.Invoke();
                SaveData();
            }
        }

        public string CurrentLocalization
        {
            get
            {
                return _currentLocalization;
            }
            set
            {
                _currentLocalization = value;
                LanguageChange?.Invoke(_currentLocalization);
            }
        }

        public bool IsDataLoaded { get; private set; } = false;

        public event UnityAction MusicStatusChange;
        public event UnityAction SFXStatusChange;
        public event UnityAction<int> MoneyChanged;
        public event UnityAction<string> LanguageChange; 

        public void Init()
        {
            if (Instance == null)
                Instance = this;
            
            LoadData();
        }

        public void Dispose()
        {
            SaveData();
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt(MoneyKey, _money);
            PlayerPrefs.SetInt(LevelKey, _level);
            PlayerPrefs.SetInt(MusicKey, Convert.ToInt32(_isMusicOn));
            PlayerPrefs.SetInt(SFXKey, Convert.ToInt32(_isSFXOn));
            PlayerPrefs.SetString(LocalizationKey, _currentLocalization);

            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : MoneyDefault;
            _money = 10000000;
            _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : LevelDefault;
            _level = 1;
            _isMusicOn = PlayerPrefs.HasKey(MusicKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(MusicKey)) : MusicDefault;
            _isSFXOn = PlayerPrefs.HasKey(SFXKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(SFXKey)) : SFXDefault;
            _currentLocalization = PlayerPrefs.HasKey(LocalizationKey) ? PlayerPrefs.GetString(LocalizationKey) : Language.ENG;

            IsDataLoaded = true;
        }

#if UNITY_EDITOR
        public void ResetData()
        {
            _money = MoneyDefault;
            _level = LevelDefault;
            _isMusicOn = MusicDefault;
            _isSFXOn = SFXDefault;
            
            SaveData();
        }
#endif
    }
}
