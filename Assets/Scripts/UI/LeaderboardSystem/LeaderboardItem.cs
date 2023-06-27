using TMPro;
using UI.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LeaderboardItem : MonoBehaviour
    {
        private readonly string _leaderboardMedalKey = "LeaderboardMedal";
        private readonly string _leaderboardCountryKey = "LeaderboardCountry";

        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _medal;
        [SerializeField] private Image _country;
        [SerializeField] private TMP_Text _nickName;
        [SerializeField] private TMP_Text _score;

        public void Init(LeaderboardData data)
        {
            _level.text = data.Level.ToString();
            ChooseMedal(data.Level);
            ChooseCountry(data.Language);
            _nickName.text = data.NickName;
            _score.text = data.Score.ToString();
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