using Assets.Scripts.UI.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LeaderboardSystem
{
    public class LeaderboardItem : MonoBehaviour
    {
        private readonly string _leaderboardMedalKey = "LeaderboardMedal";
        private readonly string _leaderboardCountryKey = "LeaderboardCountry";

        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _nickName;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Image _medal;
        [SerializeField] private Image _country;
        [SerializeField] private Image _profilePicture;

        public void Initialize(LeaderboardData data)
        {
            _level.text = data.Rank.ToString();
            _nickName.text = data.NickName;
            _score.text = data.Score.ToString();
            
            ChooseMedal(data.Rank);
            ChooseCountry(data.Language);
            //_profilePicture.sprite = data.Picture;
            Debug.Log($"[profilePicture] {data.Picture}");
        }

        private void ChooseMedal(int level)
        {
            if (level < 4)
            {
                var medals = Resources.Load<LeaderboardMedal>(_leaderboardMedalKey);
                _medal.gameObject.SetActive(true);

                switch (level)
                {
                    case 1:
                        _medal.sprite = medals.Gold;
                        break;
                    case 2:
                        _medal.sprite = medals.Silver;
                        break;
                    case 3:
                        _medal.sprite = medals.Bronze;
                        break;
                    default:
                        _medal.gameObject.SetActive(false);
                        break;
                }
            }
        }

        private void ChooseCountry(string language)
        {
            var countryImg = Resources.Load<LeaderboardCountry>(_leaderboardCountryKey);

            switch (language)
            {
                case Language.ENG:
                    _country.sprite = countryImg.Eng;
                    break;
                case Language.RUS:
                    _country.sprite = countryImg.Rus;
                    break;
                case Language.TUR:
                    _country.sprite = countryImg.Tur;
                    break;
            }
        }
    }
}