using UnityEngine;

namespace Assets.Scripts.UI.LeaderboardSystem
{
    [CreateAssetMenu(fileName = "Leaderboard Medal", menuName = "Leaderboard/Medal", order = 52)]
    public class LeaderboardMedal : ScriptableObject
    {
        [SerializeField] private Sprite _gold;
        [SerializeField] private Sprite _silver;
        [SerializeField] private Sprite _bronze;

        public Sprite Gold => _gold;

        public Sprite Silver => _silver;

        public Sprite Bronze => _bronze;
    }
}