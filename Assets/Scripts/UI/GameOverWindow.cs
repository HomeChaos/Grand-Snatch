using System;
using Agava.YandexGames;
using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using Assets.Scripts.YandexSDK;
using IJunior.TypedScenes;
using TMPro;
using UI.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        [Space] [Header("Panel Result")] 
        [SerializeField] private TMP_Text _textLevel;
        [SerializeField] private TMP_Text _textRevard;
        [SerializeField] private Button _buttonIncreaseRevenue;
        [SerializeField] private YandexAd _yandexAd;

        [Space] [Header("Panel Ranking")] 
        [SerializeField] private Button _buttonNextLevel;
        [SerializeField] private LeaderboardItem _playerRanking;
        [SerializeField] private GameObject _leaderboardItemTemplate;
        [SerializeField] private GameObject _content;
        [SerializeField] private YandexLeadboardSystem _yandexLeadboard;

        private void OnEnable()
        {
            Sound.Instance.PlayUISFX(CollectionOfSounds.Win);
            _buttonIncreaseRevenue.onClick.AddListener(WatchAdForMultiplicationOfReward);
            _buttonNextLevel.onClick.AddListener(LoadNextLevel);
            _particle.Play();
            _textRevard.text = NumberSeparator.SplitNumber(PaymentSystem.Instance.EarningsPerLevel);
            UpdateLevel();
            FillLeadboard();
        }

        private void OnDisable()
        {
            _buttonIncreaseRevenue.onClick.RemoveListener(WatchAdForMultiplicationOfReward);
            _buttonNextLevel.onClick.RemoveListener(LoadNextLevel);
        }

        private void WatchAdForMultiplicationOfReward()
        {
            _yandexAd.OnShowVideoButtonClick(OnRewarded);
        }
        
        private void OnRewarded()
        {
            Debug.Log("[!] Я получил награду!");
            int additionalMoney = PaymentSystem.Instance.EarningsPerLevel;
            _textRevard.text = NumberSeparator.SplitNumber(PaymentSystem.Instance.EarningsPerLevel * 2);
            _buttonIncreaseRevenue.gameObject.SetActive(false);

            if (PlayerData.Instance.Money + additionalMoney < 0)
                PlayerData.Instance.Money = Int32.MaxValue;
            else
                PlayerData.Instance.Money += additionalMoney;
        }

        private void UpdateLevel()
        {
            PlayerData.Instance.Level += 1;
            _textLevel.text = PlayerData.Instance.Level.ToString();
            _yandexLeadboard.SetLeaderboardScore(PlayerData.Instance.Level);
        }

        private void FillLeadboard()
        {
            FillLeadboradOfPlayer();
            FillLeadboradOfPlayers();
        }

        private void FillLeadboradOfPlayer()
        {
            var leaderboardPlayerEntry = _yandexLeadboard.GetLeaderboardPlayerEntry();
            var playerData = ConvertLeaderboradData(leaderboardPlayerEntry);
            _playerRanking.Init(playerData);
        }

        private void FillLeadboradOfPlayers()
        {
            var entries = _yandexLeadboard.GetLeaderboardEntries();
            int maxNumberOfRecords = 10;
            int currentNumberOfRecords = 0;

            foreach (var entry in entries)
            {
                var playerData = ConvertLeaderboradData(entry);
                var item = Instantiate(_leaderboardItemTemplate, _content.transform).GetComponent<LeaderboardItem>();
                item.Init(playerData);

                currentNumberOfRecords++;
                
                if (currentNumberOfRecords == maxNumberOfRecords)
                    break;
            }
        }

        private LeaderboardData ConvertLeaderboradData(LeaderboardEntryResponse data)
        {
            LeaderboardData newData = new LeaderboardData(
                data.rank, 
                Language.DefineLanguage(data.player.lang),
                data.player.publicName,
                data.score);

            return newData;
        }

        private void LoadNextLevel()
        {
            PlayerData.Instance.SaveData();
            Level_1.Load();
        }
    }
}