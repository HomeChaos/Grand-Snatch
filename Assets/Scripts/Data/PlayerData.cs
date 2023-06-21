using System;
using System.Collections.Generic;
using UI.Localization;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Data
{
    public class PlayerData : MonoBehaviour, IDisposable
    {
        [NonSerialized] public Dictionary<int, int> Cars = new Dictionary<int, int>
        {
            {0, 0}, 
            {1, 1},
            {2, 1},
            {3, 1},
            {4, 1},
            {5, 1},
            {6, 1},
            {7, 1},
            {8, 10},
            {9, 20}
        };

        private const string MoneyKey = "Money";
        private const string LevelKey = "Level";
        private const string MusicKey = "Music";
        private const string SFXKey = "SFX";
        private const string LocalizationKey = "Localization";
        private const string SelectedCarKey = "Car";
        
        private const int MoneyDefault = 100;
        private const int LevelDefault = 7;
        private const bool MusicDefault = true;
        private const bool SFXDefault = true;
        private const int SelectedCarDefault = 0;
        
        public static PlayerData Instance { get; private set; }

        [SerializeField] private Config _config;
        
        private int _money;
        private int _level;
        private bool _isMusicOn;
        private bool _isSFXOn;
        private string _currentLocalization;
        private int _selectedCar;

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

        public int SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if (0 <= value)
                    _selectedCar = value;
                else
                    throw new RankException("Incorrect value of car type!");
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
            PlayerPrefs.SetInt(SelectedCarKey, _selectedCar);

            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : MoneyDefault;
            _money = int.MaxValue;
            _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : LevelDefault;
            _level = 1;
            _isMusicOn = PlayerPrefs.HasKey(MusicKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(MusicKey)) : MusicDefault;
            _isSFXOn = PlayerPrefs.HasKey(SFXKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(SFXKey)) : SFXDefault;
            _currentLocalization = PlayerPrefs.HasKey(LocalizationKey) ? PlayerPrefs.GetString(LocalizationKey) : Language.ENG;
            _selectedCar = PlayerPrefs.HasKey(SelectedCarKey) ? PlayerPrefs.GetInt(SelectedCarKey) : SelectedCarDefault;

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
