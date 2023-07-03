using System.Collections.Generic;
using Agava.YandexGames;
using Assets.Scripts.Data;
using Assets.Scripts.MainCore;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LeaderboardSystem
{
    public class YandexLeaderboard : MonoBehaviour
    {
        private const string LeaderboardKey = "GrandSnatchLB";

        [SerializeField] private Button _showLeaderboard;
        [SerializeField] private Button _closeLeaderboard;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private int _maxNumberOfPlayersInLeaderborad = 10;
        [SerializeField] private LeaderboardItem _playerRanking;
        [SerializeField] private LeaderboardView _leaderboardView;

        private void OnEnable()
        {
            _showLeaderboard.onClick.AddListener(ShowLeaderboard);
            _closeLeaderboard.onClick.AddListener(CloseLeaderboard);
        }

        private void OnDisable()
        {
            _showLeaderboard.onClick.RemoveListener(ShowLeaderboard);
            _closeLeaderboard.onClick.RemoveListener(CloseLeaderboard);
        }

        private void CloseLeaderboard()
        {
            _leaderboardView.gameObject.SetActive(false);
            
            if (_cameraMovement != null)
                _cameraMovement.enabled = true;
        }

        private void ShowLeaderboard()
        {
            _leaderboardView.gameObject.SetActive(true);
            
            if (_cameraMovement != null)
                _cameraMovement.enabled = false;
            
            Authorized();
        }

        private void Authorized()
        {
            PlayerAccount.Authorize(
                onSuccessCallback: () =>
                {
                    PlayerAccount.RequestPersonalProfileDataPermission();
                    AddPlayerToLeaderboard();
                    FormListOfPlayers();
                    DisplayPlayersResults();
                },
                onErrorCallback: (error) =>
                {
                    _leaderboardView.gameObject.SetActive(false);
                });

            Debug.Log("Authorized: yes");
        }
        
        private void AddPlayerToLeaderboard()
        {
        
            Leaderboard.SetScore(LeaderboardKey, PlayerData.Instance.Level);         
            Debug.Log("AddPlayerToLeaderboard: yes");
        }

        private void DisplayPlayersResults()
        {
            if (PlayerAccount.IsAuthorized == false)
            {
                _playerRanking.Initialize(new LeaderboardData(
                    rank:0, 
                    language: PlayerData.Instance.CurrentLocalization, 
                    nickName: "Anonymous", 
                    score: PlayerData.Instance.Level, 
                    picture: ""));
            }
            else
            {
                Leaderboard.GetPlayerEntry(LeaderboardKey, (result) =>
                {
                    int rank = result.rank;
                    string language = result.player.lang;
                    string nickName = GetName(result.player.publicName);
                    int score = result.score;
                    string picture = result.player.profilePicture;
            
                    _playerRanking.Initialize(new LeaderboardData(rank, language, nickName, score, picture));
                });
            }
            
            Debug.Log("DisplayPlayersResults: yes");
        }

        private void FormListOfPlayers()
        {
            List<LeaderboardData> players = new List<LeaderboardData>();

            Leaderboard.GetEntries(LeaderboardKey, (result) =>
            {
                int resultAmount = result.entries.Length;
                resultAmount = Mathf.Clamp(result.entries.Length, 1, _maxNumberOfPlayersInLeaderborad);

                for (int i = 0; i < resultAmount; i++)
                {
                    Debug.Log("for in FormListOfPlayers");
                    var entry = result.entries[i];
                    
                    int rank = entry.rank;
                    string language = entry.player.lang;
                    string nickName = GetName(entry.player.publicName);
                    int score = entry.score;
                    string picture = entry.player.profilePicture;
                    
                    players.Add(new LeaderboardData(rank, language, nickName, score, picture));
                }
                
                _leaderboardView.ConstructLeaderboard(players);
            });
            
            Debug.Log("FormListOfPlayers: yes");
        }

        private string GetName(string publicName)
        {
            string value = publicName;
            
            if (string.IsNullOrEmpty(value))
                value = "Anonymous";

            return value;
        }
    }
}