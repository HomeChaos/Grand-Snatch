using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using IJunior.TypedScenes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MainCore
{
    public class LevelManager : MonoBehaviour
    {
        private Dictionary<int, Action> _levels = new Dictionary<int, Action>
        {
            {2, () => Level_2.Load()},
            {3, () => Level_3.Load()},
            {4, () => Level_4.Load()},
            {5, () => Level_5.Load()},
        };
        
        public void LoadNextLevel()
        {
            int currentLevel = PlayerData.Instance.Level;

            if (currentLevel == 1)
                Level_1.Load();
            else
                ChoseNextLevel(currentLevel);
        }

        private void ChoseNextLevel(int currentLevel)
        {
            if (_levels.ContainsKey(currentLevel))
            {
                _levels[currentLevel]();
            }
            else
            {
                var levelKeys = _levels.Keys.ToArray();
                var randomIndex = Random.Range(0, levelKeys.Length);
                var numberOfLevel = levelKeys[randomIndex];
                _levels[numberOfLevel]();
            }
        }
    }
}