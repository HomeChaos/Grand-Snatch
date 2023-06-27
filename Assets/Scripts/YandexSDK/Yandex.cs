using UnityEngine;
using System.Collections;
using Agava.YandexGames;

namespace Assets.Scripts.YandexSDK
{
    public class Yandex : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.CallbackLogging = true;
            
            if (YandexGamesSdk.IsInitialized == false)
                StartCoroutine(OnStart());
#endif
        }

        private IEnumerator OnStart()
        {
            yield return YandexGamesSdk.Initialize();
        }
    }
}