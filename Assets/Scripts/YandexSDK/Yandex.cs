using UnityEngine;
using System.Collections;
using Agava.YandexGames;

namespace Assets.Scripts.YandexSDK
{
    public class Yandex : MonoBehaviour
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
            
            if (YandexGamesSdk.IsInitialized == false)
                StartCoroutine(OnStart());
        }

        private IEnumerator OnStart()
        {
            yield return YandexGamesSdk.Initialize();
        }
    }
}