using UnityEngine;
using System.Collections;
using Agava.YandexGames;
using Assets.Scripts.Data;
using IJunior.TypedScenes;

namespace Assets.Scripts.YandexSDK
{
    public class Yandex : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
#if UNITY_EDITOR
            LoadPlayerData();
            yield break;
#endif
            yield return YandexGamesSdk.Initialize(LoadPlayerData);
        }

        private void LoadPlayerData()
        {
            _playerData.Initialize();
        }
    }
}