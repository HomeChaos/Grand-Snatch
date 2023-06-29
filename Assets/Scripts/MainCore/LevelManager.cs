using Assets.Scripts.Data;
using IJunior.TypedScenes;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadNextLevel()
        {
            int currentLevel = PlayerData.Instance.Level;

            if (currentLevel == 1)
            {
                Level_1.Load();
            }
            else
            {
                ChoseRandomLevel();
            }
        }

        private void ChoseRandomLevel()
        {
            Level_2.Load();
        }
    }
}