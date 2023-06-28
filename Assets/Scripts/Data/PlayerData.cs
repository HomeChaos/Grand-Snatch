using System;
using System.Collections.Generic;
using Agava.YandexGames;
using Assets.Scripts.Shop;
using Assets.Scripts.UI.Localization;
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

        #region Propertys        
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
                SaveMoney();
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

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

        public event UnityAction MusicStatusChange;
        public event UnityAction SFXStatusChange;
        public event UnityAction<int> MoneyChanged;
        public event UnityAction<string> LanguageChange;

        #endregion

        public void Initialize()
        {
            Instance = this;
            DontDestroyOnLoad(this);
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


        [ContextMenu("Delete Data")]
        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(MoneyKey);
            PlayerPrefs.DeleteKey(LevelKey);
            PlayerPrefs.DeleteKey(MusicKey);
            PlayerPrefs.DeleteKey(SFXKey);
            PlayerPrefs.DeleteKey(LocalizationKey);
            PlayerPrefs.DeleteKey(SelectedCarKey);
            PlayerPrefs.DeleteKey(ConditionsForCarsKey);
        }

        [ContextMenu("Reset Data")]
        public void ResetData()
        {
            _money = MoneyDefault;
            _level = LevelDefault;
            _isMusicOn = MusicDefault;
            _isSFXOn = SFXDefault;
            _currentLocalization = DetermineBrowserLanguage();
            _selectedCar = SelectedCarDefault;
            LoadConditionsFromPriceList();
            
            SaveData();
        }

        private void LoadData()
        {
            _config = Resources.Load<Config>("GameConfig");
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : MoneyDefault;
            _money = 100_000;
            _level = PlayerPrefs.HasKey(LevelKey) ? PlayerPrefs.GetInt(LevelKey) : LevelDefault;
            _isMusicOn = PlayerPrefs.HasKey(MusicKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(MusicKey)) : MusicDefault;
            _isSFXOn = PlayerPrefs.HasKey(SFXKey) ? Convert.ToBoolean(PlayerPrefs.GetInt(SFXKey)) : SFXDefault;
            _currentLocalization = PlayerPrefs.HasKey(LocalizationKey) ? PlayerPrefs.GetString(LocalizationKey) : DetermineBrowserLanguage();
            _selectedCar = PlayerPrefs.HasKey(SelectedCarKey) ? PlayerPrefs.GetInt(SelectedCarKey) : SelectedCarDefault;
            LoadConditionsForCars();

            LoadMainMenu();
        }
        
        private void LoadMainMenu()
        {
            IJunior.TypedScenes.MainMenu.Load();
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

        private void SaveMoney()
        {
            PlayerPrefs.SetInt(MoneyKey, _money);
            PlayerPrefs.Save();
        }

        private string DetermineBrowserLanguage()
        {
#if !UNITY_EDITOR
            return Language.DefineLanguage(YandexGamesSdk.Environment.i18n.lang);
#endif
            return Language.ENG;
        }
    }
}
