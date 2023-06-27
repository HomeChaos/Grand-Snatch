using System;
using Agava.YandexGames;
using Assets.Scripts.Sounds;
using UnityEngine;

namespace Assets.Scripts.YandexSDK
{
    public class YandexAd : MonoBehaviour
    {
        public void OnShowVideoButtonClick(Action onReward)
        {
            VideoAd.Show(
                onOpenCallback: Sound.Instance.Pause,
                onRewardedCallback: onReward,
                onCloseCallback: Sound.Instance.UpPause,
                onErrorCallback: (error) =>
                {
                    Debug.Log($"[error] {error}");
                    Sound.Instance.UpPause();
                }
                );
        }

        public void OnShowStickyAdButtonClick()
        {
            StickyAd.Show();
        }
    }
}