using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Shop;
using UI.Localization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Data
{
    public class PlayerData : MonoBehaviour, IDisposable
    {
        private const string MoneyKey = "Money";
        private const string LevelKey = "Level";
        private const string MusicKey = "Music";
        private const string SFXKey = "SFX";
        private const string LocalizationKey = "Localization";
        private const string SelectedCarKey = "Car";
        private const string ConditionsForCarsKey = "Conditions";
        
        private const int MoneyDefault = 100;
        private const int LevelDefault = 1;
        private const bool MusicDefault = true;
        private const bool SFXDefault = true;
        private const int SelectedCarDefault = 0;

        public static PlayerData Instance { get; private set; }

        private Config _config;
        private int _money;
        private int _level;
        private bool _isMusicOn;
        private bool _isSFXOn;
        private string _currentLocalization;
        private int _selectedCar;
        private Dictionary<CarType, int> _conditionsForCars = new Dictionary<CarType, int>();

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

        public IReadOnlyDictionary<CarType, int> ConditionsForCars => _conditionsForCars;

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

        public void ChangeConditionForCar(CarType type)
        {
            if (_conditionsForCars[type] - 1 < 0)
                throw new RankException("Incorrect value for the car condition");
                
            _conditionsForCars[type]--;
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt(MoneyKey, _money);
            PlayerPrefs.SetInt(LevelKey, _level);
            PlayerPrefs.SetInt(MusicKey, Convert.ToInt32(_isMusicOn));
            PlayerPrefs.SetInt(SFXKey, Convert.ToInt32(_isSFXOn));
            PlayerPrefs.SetString(LocalizationKey, _currentLocalization);
            PlayerPrefs.SetInt(SelectedCarKey, _selectedCar);
            PlayerPrefs.SetString(ConditionsForCarsKey, ConvertConditionsForCarsToString());

            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            _config = Resources.Load<Config>("GameConfig");
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : MoneyDefault;
            _money = 12_345;
            _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : LevelDefault;
            _isMusicOn = PlayerPrefs.HasKey(MusicKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(MusicKey)) : MusicDefault;
            _isSFXOn = PlayerPrefs.HasKey(SFXKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(SFXKey)) : SFXDefault;
            _currentLocalization = PlayerPrefs.HasKey(LocalizationKey) ? PlayerPrefs.GetString(LocalizationKey) : Language.ENG;
            _selectedCar = PlayerPrefs.HasKey(SelectedCarKey) ? PlayerPrefs.GetInt(SelectedCarKey) : SelectedCarDefault;
            LoadConditionsForCars();

            IsDataLoaded = true;
        }

        private void LoadConditionsForCars()
        {
            if (PlayerPrefs.HasKey(ConditionsForCarsKey) && PlayerPrefs.GetString(ConditionsForCarsKey).Length > 0)
            {
                string[] encryptedDate = PlayerPrefs.GetString(ConditionsForCarsKey).Split(';');
                foreach (var date in encryptedDate)
                {
                    string[] pairOfValues = date.Split(',');
                    
                    int key = int.Parse(pairOfValues[0]);
                    int value = int.Parse(pairOfValues[1]);

                    _conditionsForCars[(CarType)key] = value;
                }
            }
            else
            {
                LoadConditionsFromPriceList();
            }
        }

        private void LoadConditionsFromPriceList()
        {
            var priceList = Resources.Load<PriceList>("PriceList");

            foreach (var item in priceList.Prices)
            {
                CarType carType = item.CarType;
                int conditions = item.IsBuyForAd ? item.Cost : 1;
                _conditionsForCars[carType] = conditions;
            }

            _conditionsForCars[CarType.Base] = 0;
        }

        private string ConvertConditionsForCarsToString()
        {
            var data = new List<string>();

            foreach (var conditions in _conditionsForCars)
            {
                data.Add($"{(int)conditions.Key},{conditions.Value}");
            }
            
            string result = String.Join(';', data.ToArray());
            return result;
        }


#if UNITY_EDITOR
        [ContextMenu("Reset Data")]
        private void ResetData()
        {
            _money = MoneyDefault;
            _level = LevelDefault;
            _isMusicOn = MusicDefault;
            _isSFXOn = SFXDefault;
            _currentLocalization = Language.ENG;
            _selectedCar = SelectedCarDefault;
            LoadConditionsFromPriceList();
            
            SaveData();
        }
#endif
    }
}
