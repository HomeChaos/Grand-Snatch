using UnityEngine;

namespace Assets.Scripts.UI
{
    [CreateAssetMenu(fileName = "LeaderboardCountry", menuName = "Leaderboard/Country", order = 52)]
    public class LeaderboardCountry : ScriptableObject
    {
        [SerializeField] private Sprite _eng;
        [SerializeField] private Sprite _rus;
        [SerializeField] private Sprite _tur;

        public Sprite Eng => _eng;

        public Sprite Rus => _rus;

        public Sprite Tur => _tur;
    }
}