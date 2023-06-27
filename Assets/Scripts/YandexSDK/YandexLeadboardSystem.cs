using Agava.YandexGames;
using UnityEngine;

namespace Assets.Scripts.YandexSDK
{
    public class YandexLeadboardSystem : MonoBehaviour
    {
        private const string LeaderboardKey = "GrandSnatchLB";
        
        public void SetLeaderboardScore(int level)
        {
            Leaderboard.SetScore(LeaderboardKey, level);
        }

        public LeaderboardEntryResponse[] GetLeaderboardEntries()
        {
            var usersLeaderboard = new LeaderboardEntryResponse[] {};
            
            Leaderboard.GetEntries(LeaderboardKey, (result) =>
            {
                usersLeaderboard = result.entries;
            });

            return usersLeaderboard;
        }

        public LeaderboardEntryResponse GetLeaderboardPlayerEntry()
        {
            var userLeaderboard = new LeaderboardEntryResponse();
            
            Leaderboard.GetPlayerEntry(LeaderboardKey, (result) =>
            {
                userLeaderboard = result;
            });

            return userLeaderboard;
        }
    }
}
