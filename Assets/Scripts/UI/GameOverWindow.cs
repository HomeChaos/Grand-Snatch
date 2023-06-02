using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private Button _nextLevel;

        private void OnEnable()
        {
            _nextLevel.onClick.AddListener(LoadNextLevel);
        }

        private void OnDisable()
        {
            _nextLevel.onClick.RemoveListener(LoadNextLevel);
        }

        private void LoadNextLevel()
        {
            Level_1.Load();
        }
    }
}