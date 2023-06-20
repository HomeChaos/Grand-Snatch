using Assets.Scripts.Data;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private Button _nextLevel;
        [SerializeField] private ParticleSystem _particle;

        private void OnEnable()
        {
            _nextLevel.onClick.AddListener(LoadNextLevel);
            _particle.Play();
        }

        private void OnDisable()
        {
            _nextLevel.onClick.RemoveListener(LoadNextLevel);
        }

        private void LoadNextLevel()
        {
            PlayerData.Instance.SaveData();
            Level_1.Load();
        }
    }
}