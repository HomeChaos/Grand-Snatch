using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.LeaderboardSystem
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderboardItemTemplate;
        [SerializeField] private Transform _content;
        
        private List<GameObject> _spawnedElements = new List<GameObject>();

        public void ConstructLeaderboard(List<LeaderboardData> playersInfo)
        {
            ClearLeaderboard();

            foreach (LeaderboardData data in playersInfo)
            {
                GameObject leaderboardElementInstance = Instantiate(_leaderboardItemTemplate, _content);
                
                LeaderboardItem leaderboardElement = leaderboardElementInstance.GetComponent<LeaderboardItem>();
                leaderboardElement.Initialize(data);
                
                _spawnedElements.Add(leaderboardElementInstance);
            }
        }

        private void ClearLeaderboard()
        {
            foreach (var element in _spawnedElements)
                Destroy(element);

            _spawnedElements = new List<GameObject>();
        }
    }
}