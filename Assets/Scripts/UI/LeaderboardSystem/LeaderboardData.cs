namespace Assets.Scripts.UI
{
    public class LeaderboardData
    {
        public int Level { get; private set; }
        public string Language { get; private set; }
        public string NickName  { get; private set; }
        public int Score { get; private set; }

        public LeaderboardData(int level, string language, string nickName, int score)
        {
            Level = level;
            Language = language;
            NickName = nickName;
            Score = score;
        }
    }
}